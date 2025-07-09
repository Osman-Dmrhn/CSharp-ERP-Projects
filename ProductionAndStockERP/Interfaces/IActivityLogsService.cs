using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IActivityLogsService
    {     
        Task<ResponseHelper<bool>> AddLogAsync(int userId, string action);
        Task<ResponseHelper<IEnumerable<ActivityLogs>>> GetAllLogsAsync();
        Task<ResponseHelper<IEnumerable<ActivityLogs>>> GetLogsByUserIdAsync(int userId);
        Task<ResponseHelper<ActivityLogs>> GetLogByIdAsync(int logId);
        Task<ResponseHelper<bool>> DeleteLogAsync(int logId);
    }
}
