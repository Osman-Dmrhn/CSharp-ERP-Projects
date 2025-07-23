// Dosya: Services/ProductionOrderService.cs
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class ProductionOrderService : IProductionOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService; // YENİ: Log servisi enjekte edildi.

        public ProductionOrderService(ApplicationDbContext context, IActivityLogsService activityLogsService)
        {
            _context = context;
            _activityLogsService = activityLogsService; // YENİ: DI ile enjekte edildi.
        }

        public async Task<ResponseHelper<ProductionOrder>> CreatePrdouctionOrderAsync(ProductionOrder proOrder, int performingUserId)
        {
            await _context.ProductionOrders.AddAsync(proOrder);
            await _context.SaveChangesAsync();

            await _activityLogsService.AddLogAsync(performingUserId, "Yeni üretim emri oluşturuldu.", "Başarılı", "ProductionOrder", proOrder.ProductionId.ToString());

            return ResponseHelper<ProductionOrder>.Ok(proOrder);
        }

        public async Task<ResponseHelper<ProductionOrder>> UpdatePrdouctionOrderAsync(ProductionOrder updatedProOrder, int performingUserId)
        {
            var existingOrder = await _context.ProductionOrders.FindAsync(updatedProOrder.ProductionId);
            if (existingOrder == null)
            {
                return ResponseHelper<ProductionOrder>.Fail("Güncellenecek üretim emri bulunamadı.");
            }

            // Değişiklikleri tespit et
            var changes = new Dictionary<string, object>();
            if (existingOrder.ProductName != updatedProOrder.ProductName)
                changes["ProductName"] = new { Old = existingOrder.ProductName, New = updatedProOrder.ProductName };
            if (existingOrder.Quantity != updatedProOrder.Quantity)
                changes["Quantity"] = new { Old = existingOrder.Quantity, New = updatedProOrder.Quantity };
            if (existingOrder.Status != updatedProOrder.Status)
                changes["Status"] = new { Old = existingOrder.Status.ToString(), New = updatedProOrder.Status.ToString() };

            // Veritabanı nesnesini güncelle
            existingOrder.ProductName = updatedProOrder.ProductName;
            existingOrder.Quantity = updatedProOrder.Quantity;
            existingOrder.Status = updatedProOrder.Status;

            await _context.SaveChangesAsync();

            // Değişiklikleri JSON'a çevir ve logla
            string changesJson = changes.Count > 0 ? JsonSerializer.Serialize(changes) : null;
            await _activityLogsService.AddLogAsync(performingUserId, "Üretim emri güncellendi.", "Başarılı", "ProductionOrder", existingOrder.ProductionId.ToString(), changesJson);

            return ResponseHelper<ProductionOrder>.Ok(existingOrder);
        }

        public async Task<ResponseHelper<bool>> DeletePrdouctionOrderAsync(int id, int performingUserId)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null)
            {
                await _activityLogsService.AddLogAsync(performingUserId, $"ID'si {id} olan üretim emrini silme denemesi başarısız.", "Başarısız", "ProductionOrder", id.ToString());
                return ResponseHelper<bool>.Fail("Üretim emri bulunamadı.");
            }

            _context.ProductionOrders.Remove(order);
            await _context.SaveChangesAsync();

            await _activityLogsService.AddLogAsync(performingUserId, "Üretim emri silindi.", "Başarılı", "ProductionOrder", id.ToString());

            return ResponseHelper<bool>.Ok(true);
        }

        // Get metotları aynı kalabilir
        public async Task<ResponseHelper<IEnumerable<ProductionOrder>>> GetAllPrdouctionOrderAsync()
        {
            var result = await _context.ProductionOrders.ToListAsync();
            return ResponseHelper<IEnumerable<ProductionOrder>>.Ok(result);
        }

        public async Task<ResponseHelper<ProductionOrder>> GetPrdouctionOrderByIdAsync(int id)
        {
            var result = await _context.ProductionOrders.FindAsync(id);
            if (result == null) return ResponseHelper<ProductionOrder>.Fail("Üretim emri bulunamadı.");
            return ResponseHelper<ProductionOrder>.Ok(result);
        }
    }
}