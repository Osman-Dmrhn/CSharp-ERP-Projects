using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IProductionOrderService
    {
        Task<ResponseHelper<IEnumerable<ProductionOrder>>> GetAllPrdouctionOrderAsync();
        Task<ResponseHelper<ProductionOrder>> GetPrdouctionOrderByIdAsync(int id);
        Task<ResponseHelper<ProductionOrder>> CreatePrdouctionOrderAsync(ProductionOrder proOrder, int performingUserId);
        Task<ResponseHelper<ProductionOrder>> UpdatePrdouctionOrderAsync(ProductionOrder proOrder, int performingUserId);
        Task<ResponseHelper<bool>> DeletePrdouctionOrderAsync(int id, int performingUserId);
    }
}
