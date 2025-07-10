using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Services
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseHelper<IEnumerable<Order>>> GetAllOrdersAsync()
        {
            var result = await _context.Orders.ToListAsync();
            return ResponseHelper<IEnumerable<Order>>.Ok(result);
        }
        public async Task<ResponseHelper<Order>> GetOrderByIdAsync(int id)
        {
            var result = await _context.Orders.FindAsync(id);
            if (result == null)
            {
                return ResponseHelper<Order>.Fail("Ürün Bulunamadı");
            }
            return ResponseHelper<Order>.Ok(result);
        }

        public async Task<ResponseHelper<bool>> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return ResponseHelper<bool>.Ok(true);
        }
        public async Task<ResponseHelper<bool>> UpdateOrderAsync(Order order)
        {
            var result= await _context.Orders.FindAsync(order.OrderId);
            if (result is not null)
            {
                result.ProductName = order.ProductName;
                result.Quantity = order.Quantity;
                result.Status= order.Status;
                result.CustomerName = order.CustomerName;

                _context.Orders.Update(result);
                await _context.SaveChangesAsync();
                return ResponseHelper<bool>.Ok(true);
            }
            else
                return ResponseHelper<bool>.Fail("Ürün Bulunamadı");
        }
        public async Task<ResponseHelper<bool>> DeleteOrderAsync(int id)
        {
            var result = await _context.Orders.FindAsync(id);
            if (result is not null)
            {
                _context.Orders.Remove(result);
                await _context.SaveChangesAsync();
                return ResponseHelper<bool>.Ok(true);
            }
            else
                return ResponseHelper<bool>.Fail("Ürün Bulunamadı");
        }
    }
}
