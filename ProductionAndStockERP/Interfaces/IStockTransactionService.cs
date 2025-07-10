using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IStockTransactionService
    {
        Task<ResponseHelper<IEnumerable<StockTransaction>>> GetAllStockTransactionAsync();
        Task<ResponseHelper<StockTransaction>> GetStockTransactionByIdAsync(int id);

        Task<ResponseHelper<bool>> CreateStockTransactionAsync(StockTransaction stockT);
        Task<ResponseHelper<bool>> UpdateStockTransactionAsync(StockTransaction stockT);
        Task<ResponseHelper<bool>> DeleteStockTransactionAsync(int id);
    }
}
