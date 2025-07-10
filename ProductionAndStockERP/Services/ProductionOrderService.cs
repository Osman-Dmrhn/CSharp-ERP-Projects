using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Services
{
    public class ProductionOrderService : IProductionOrderService
    {
        private readonly ApplicationDbContext _context;
        public ProductionOrderService(ApplicationDbContext context)
        {
            _context = context;
        }
         async Task<ResponseHelper<IEnumerable<ProductionOrder>>> IProductionOrderService.GetAllPrdouctionOrderAsync()
        {
            var result = await _context.ProductionOrders.ToListAsync();
            return ResponseHelper<IEnumerable<ProductionOrder>>.Ok(result);
        }

         async Task<ResponseHelper<ProductionOrder>> IProductionOrderService.GetPrdouctionOrderByIdAsync(int id)
        {
            var result = await _context.ProductionOrders.FindAsync(id);
            if (result == null) return ResponseHelper<ProductionOrder>.Fail("PrdouctionOrder Bulunamadı");
            return ResponseHelper<ProductionOrder>.Ok(result);
        }
        async Task<ResponseHelper<bool>> IProductionOrderService.CreatePrdouctionOrderAsync(ProductionOrder proOrder)
        {
            await _context.ProductionOrders.AddAsync(proOrder);
            await _context.SaveChangesAsync();
            return ResponseHelper<bool>.Ok(true);
        }

        async Task<ResponseHelper<bool>> IProductionOrderService.DeletePrdouctionOrderAsync(int id)
        {
            var result = await _context.ProductionOrders.FindAsync(id);
            if (result == null) return ResponseHelper<bool>.Fail("PrdouctionOrder Bulunamadı");

            _context.ProductionOrders.Remove(result);
            await _context.SaveChangesAsync();
            return ResponseHelper<bool>.Ok(true);
        }

        async Task<ResponseHelper<bool>> IProductionOrderService.UpdatePrdouctionOrderAsync(ProductionOrder proOrder)
        {
            var result = await _context.ProductionOrders.FindAsync(proOrder.ProductionId);
            if (result == null) return ResponseHelper<bool>.Fail("PrdouctionOrder Bulunamadı");

            result.Quantity= proOrder.Quantity;
            result.ProductName= proOrder.ProductName;
            result.Status= proOrder.Status;

            _context.ProductionOrders.Update(result);
            await _context.SaveChangesAsync();
            return ResponseHelper<bool>.Ok(true);
        }
    }
}
