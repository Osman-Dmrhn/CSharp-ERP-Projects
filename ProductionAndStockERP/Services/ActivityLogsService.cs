using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
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

        public async Task<ResponseHelper<bool>> AddLogAsync(int userId, string action)
        {
            var log = new ActivityLogs
            {
                UserId = userId,
                Action = action,
                CreatedAt = DateTime.UtcNow
            };

            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            return ResponseHelper<bool>.Ok(true);
        }
        public async Task<ResponseHelper<IEnumerable<ActivityLogs>>> GetAllLogsAsync()
        {
            var logs = await _context.ActivityLogs.ToListAsync();
            return ResponseHelper<IEnumerable<ActivityLogs>>.Ok(logs);
        }
        public async Task<ResponseHelper<IEnumerable<ActivityLogs>>> GetLogsByUserIdAsync(int userId)
        {
            var logs = await _context.ActivityLogs.Where(log => log.UserId == userId).ToListAsync();
            return ResponseHelper<IEnumerable<ActivityLogs>>.Ok(logs);
        }
        public async Task<ResponseHelper<ActivityLogs>> GetLogByIdAsync(int logId)
        {
            var log = await _context.ActivityLogs.FindAsync(logId);
            if (log == null)
            {
                return ResponseHelper<ActivityLogs>.Fail("Log bulunamadı.");
            }

            return ResponseHelper<ActivityLogs>.Ok(log);
        }

        public async Task<ResponseHelper<bool>> DeleteLogAsync(int logId)
        {
            var log = await _context.ActivityLogs.FindAsync(logId);
            if (log == null)
            {
                return ResponseHelper<bool>.Fail("Log bulunamadı.");
            }

            _context.ActivityLogs.Remove(log);
            await _context.SaveChangesAsync();

            return ResponseHelper<bool>.Ok(true);
        }
    }
}

