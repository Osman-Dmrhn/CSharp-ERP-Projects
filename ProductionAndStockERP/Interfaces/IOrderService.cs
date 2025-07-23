// Dosya: Interfaces/IOrderService.cs
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseHelper<IEnumerable<Order>>> GetAllOrdersAsync();
        Task<ResponseHelper<Order>> GetOrderByIdAsync(int id);
        Task<ResponseHelper<Order>> CreateOrderAsync(Order order, int performingUserId);
        Task<ResponseHelper<Order>> UpdateOrderAsync(Order order, int performingUserId);
        Task<ResponseHelper<bool>> DeleteOrderAsync(int id, int performingUserId);
    }
}