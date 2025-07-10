using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IProductionOrderService
    {
        Task<ResponseHelper<IEnumerable<ProductionOrder>>> GetAllPrdouctionOrderAsync();
        Task<ResponseHelper<ProductionOrder>> GetPrdouctionOrderByIdAsync(int id);
        Task<ResponseHelper<bool>> CreatePrdouctionOrderAsync(ProductionOrder proOrder);
        Task<ResponseHelper<bool>> UpdatePrdouctionOrderAsync(ProductionOrder proOrder);
        Task<ResponseHelper<bool>> DeletePrdouctionOrderAsync(int id);
    }
}
