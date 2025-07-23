// Dosya: Services/ActivityLogsService.cs
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.ActivityLogDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Services
{
    public class ActivityLogsService : IActivityLogsService
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseHelper<bool>> AddLogAsync(int userId, string action, string status, string targetEntity = null, string targetEntityId = null, string changes = null)
        {
            var log = new ActivityLogs
            {
                UserId = userId,
                Action = action,
                CreatedAt = DateTime.UtcNow,
                Status = status,
                TargetEntity = targetEntity,
                TargetEntityId = targetEntityId,
                Changes = changes
            };

            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();
            return ResponseHelper<bool>.Ok(true);
        }

        public async Task<ResponseHelper<PagedResponse<ActivityLogDto>>> GetAllLogsAsync(LogFilterParameters filters)
        {
            var query = _context.ActivityLogs.Include(log => log.User).AsQueryable();

            if (filters.StartDate.HasValue)
                query = query.Where(l => l.CreatedAt >= filters.StartDate.Value);

            query = query.OrderByDescending(l => l.CreatedAt);

            var dtoQuery = query.Select(log => new ActivityLogDto
            {
                Id = log.LogId,
                UserName = log.User.UserName,
                Action = log.Action,
                CreatedAt = log.CreatedAt,
                Status = log.Status,
                TargetEntity = log.TargetEntity
            });

            var totalRecords = await dtoQuery.CountAsync();
            var items = await dtoQuery.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
            var pagedResponse = new PagedResponse<ActivityLogDto>(items, filters.PageNumber, filters.PageSize, totalRecords);

            return ResponseHelper<PagedResponse<ActivityLogDto>>.Ok(pagedResponse);
        }

        public async Task<ResponseHelper<ActivityLogDetailDto>> GetLogByIdAsync(int logId)
        {
            var log = await _context.ActivityLogs
                .Include(l => l.User)
                .Where(l => l.LogId == logId)
                .Select(l => new ActivityLogDetailDto
                {
                    Id = l.LogId,
                    UserId = l.UserId,
                    UserName = l.User.UserName,
                    Action = l.Action,
                    CreatedAt = l.CreatedAt,
                    Status = l.Status,
                    TargetEntity = l.TargetEntity,
                    TargetEntityId = l.TargetEntityId,
                    Changes = l.Changes
                })
                .FirstOrDefaultAsync();

            if (log == null)
            {
                return ResponseHelper<ActivityLogDetailDto>.Fail("Log bulunamadı.");
            }

            return ResponseHelper<ActivityLogDetailDto>.Ok(log);
        }
        public async Task<ResponseHelper<IEnumerable<ActivityLogDto>>> GetLogsByUserIdAsync(int userId)
        {
            var logs = await _context.ActivityLogs
                .Where(log => log.UserId == userId)
                .Include(log => log.User)
                .OrderByDescending(log => log.CreatedAt)
                .Select(log => new ActivityLogDto
                {
                    Id = log.LogId,
                    UserName = log.User.UserName,
                    Action = log.Action,
                    CreatedAt = log.CreatedAt,
                    Status = log.Status,
                    TargetEntity = log.TargetEntity
                })
                .ToListAsync();

            return ResponseHelper<IEnumerable<ActivityLogDto>>.Ok(logs);
        }

        public async Task<ResponseHelper<bool>> DeleteLogAsync(int logId)
        {
            var log = await _context.ActivityLogs.FindAsync(logId);
            if (log == null)
            {
                return ResponseHelper<bool>.Fail("Silinecek log bulunamadı.");
            }

            _context.ActivityLogs.Remove(log);
            await _context.SaveChangesAsync();

            return ResponseHelper<bool>.Ok(true);
        }
    }
}