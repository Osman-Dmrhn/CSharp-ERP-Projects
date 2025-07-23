// Dosya: Services/OrderService.cs
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService; // YENİ: Log servisi enjekte edildi.

        public OrderService(ApplicationDbContext context, IActivityLogsService activityLogsService)
        {
            _context = context;
            _activityLogsService = activityLogsService; // YENİ: DI ile enjekte edildi.
        }

        public async Task<ResponseHelper<Order>> CreateOrderAsync(Order order, int performingUserId)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Loglama işlemi burada yapılıyor.
            await _activityLogsService.AddLogAsync(performingUserId, "Yeni sipariş oluşturuldu.", "Başarılı", "Order", order.OrderId.ToString());

            return ResponseHelper<Order>.Ok(order);
        }

        public async Task<ResponseHelper<Order>> UpdateOrderAsync(Order updatedOrder, int performingUserId)
        {
            var existingOrder = await _context.Orders.FindAsync(updatedOrder.OrderId);
            if (existingOrder == null)
            {
                return ResponseHelper<Order>.Fail("Güncellenecek ürün bulunamadı.");
            }

            // Değişiklikleri tespit et
            var changes = new Dictionary<string, object>();
            if (existingOrder.ProductName != updatedOrder.ProductName)
                changes["ProductName"] = new { Old = existingOrder.ProductName, New = updatedOrder.ProductName };
            if (existingOrder.Quantity != updatedOrder.Quantity)
                changes["Quantity"] = new { Old = existingOrder.Quantity, New = updatedOrder.Quantity };
            if (existingOrder.Status != updatedOrder.Status)
                changes["Status"] = new { Old = existingOrder.Status.ToString(), New = updatedOrder.Status.ToString() };

            // Veritabanı nesnesini güncelle
            existingOrder.ProductName = updatedOrder.ProductName;
            existingOrder.Quantity = updatedOrder.Quantity;
            existingOrder.Status = updatedOrder.Status;
            existingOrder.CustomerName = updatedOrder.CustomerName;

            await _context.SaveChangesAsync();

            // Değişiklikleri JSON'a çevir ve logla
            string changesJson = changes.Count > 0 ? JsonSerializer.Serialize(changes) : null;
            await _activityLogsService.AddLogAsync(performingUserId, "Sipariş güncellendi.", "Başarılı", "Order", existingOrder.OrderId.ToString(), changesJson);

            return ResponseHelper<Order>.Ok(existingOrder);
        }

        public async Task<ResponseHelper<bool>> DeleteOrderAsync(int id, int performingUserId)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                await _activityLogsService.AddLogAsync(performingUserId, $"ID'si {id} olan siparişi silme denemesi başarısız.", "Başarısız", "Order", id.ToString());
                return ResponseHelper<bool>.Fail("Ürün bulunamadı.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            await _activityLogsService.AddLogAsync(performingUserId, "Sipariş silindi.", "Başarılı", "Order", id.ToString());

            return ResponseHelper<bool>.Ok(true);
        }

        // Get metotları aynı kalabilir
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
    }
}