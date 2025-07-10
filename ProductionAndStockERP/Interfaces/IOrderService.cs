using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseHelper<IEnumerable<Order>>> GetAllOrdersAsync();
        Task<ResponseHelper<Order>> GetOrderByIdAsync(int id);

        Task<ResponseHelper<bool>> CreateOrderAsync(Order order);
        Task<ResponseHelper<bool>> UpdateOrderAsync(Order order);
        Task<ResponseHelper<bool>> DeleteOrderAsync(int id);
    }
}
