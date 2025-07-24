
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;
using ProductionAndStockERP.Dtos.StockTransactionDtos; 

namespace ProductionAndStockERP.Interfaces
{
    public interface IStockTransactionService
    {
        Task<ResponseHelper<PagedResponse<StockTransactionDto>>> GetAllStockTransactionAsync(StockTransactionFilterParameters filters);
        Task<ResponseHelper<StockTransactionDto>> GetStockTransactionByIdAsync(int id);

        Task<ResponseHelper<StockTransaction>> CreateStockTransactionAsync(StockTransaction stockT, int performingUserId);
        Task<ResponseHelper<StockTransaction>> UpdateStockTransactionAsync(StockTransaction stockT, int performingUserId);
        Task<ResponseHelper<bool>> DeleteStockTransactionAsync(int id, int performingUserId);

    }
}