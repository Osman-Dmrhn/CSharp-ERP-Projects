using ProductionAndStockERP.Dtos.ActivityLogDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IActivityLogsService
    {
        Task<ResponseHelper<bool>> AddLogAsync(int userId, string action, string status, string targetEntity = null, string targetEntityId = null, string changes = null);
        Task<ResponseHelper<PagedResponse<ActivityLogDto>>> GetAllLogsAsync(LogFilterParameters filters);
        Task<ResponseHelper<ActivityLogDetailDto>> GetLogByIdAsync(int logId);
        Task<ResponseHelper<IEnumerable<ActivityLogDto>>> GetLogsByUserIdAsync(int userId);
        Task<ResponseHelper<bool>> DeleteLogAsync(int logId);
    }
}
