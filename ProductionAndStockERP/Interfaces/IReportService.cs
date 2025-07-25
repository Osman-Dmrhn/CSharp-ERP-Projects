using ProductionAndStockERP.Dtos.ActivityLogDtos;
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Dtos.ProductDtos;
using ProductionAndStockERP.Dtos.ProductionOrderDtos;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
using ProductionAndStockERP.Helpers;

namespace ProductionAndStockERP.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<OrderDto>> GetFilteredOrdersForReportAsync(OrderFilterParameters filters);
        Task<IEnumerable<ProductionOrderDto>> GetFilteredProductionOrdersForReportAsync(ProductionOrderFilterParameters filters);
        Task<IEnumerable<StockTransactionDto>> GetFilteredStockTransactionsForReportAsync(StockTransactionFilterParameters filters);
        Task<IEnumerable<ProductDto>> GetFilteredProductsForReportAsync(ProductFilterParameters filters);
        Task<IEnumerable<ActivityLogDto>> GetFilteredLogsForReportAsync(LogFilterParameters filters);
    }
}
