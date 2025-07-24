// Dosya: Services/ProductionOrderService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.ProductionOrderDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class ProductionOrderService : IProductionOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService;
        private readonly IMapper _mapper;

        public ProductionOrderService(ApplicationDbContext context, IActivityLogsService activityLogsService, IMapper mapper)
        {
            _context = context;
            _activityLogsService = activityLogsService;
            _mapper = mapper;
        }

        public async Task<ResponseHelper<ProductionOrder>> CreatePrdouctionOrderAsync(ProductionOrder proOrder, int performingUserId)
        {
            await _context.ProductionOrders.AddAsync(proOrder);
            await _context.SaveChangesAsync();

            await _activityLogsService.AddLogAsync(performingUserId, "Yeni üretim emri oluşturuldu.", "Başarılı", "ProductionOrder", proOrder.ProductionId.ToString());

            return ResponseHelper<ProductionOrder>.Ok(proOrder);
        }
      
        public async Task<ResponseHelper<PagedResponse<ProductionOrderDto>>> GetAllPrdouctionOrderAsync(ProductionOrderFilterParameters filters)
        {
            var query = _context.ProductionOrders
                .Include(p => p.Product) 
                .Include(p => p.User)   
                .AsQueryable();

            if (filters.ProductId.HasValue)
                query = query.Where(p => p.ProductId == filters.ProductId.Value);
            if (!string.IsNullOrEmpty(filters.Status))
                query = query.Where(p => p.Status.ToString() == filters.Status);

            query = query.OrderByDescending(p => p.CreatedAt);

            var dtoQuery = query.Select(p => new ProductionOrderDto
            {
                ProductionId = p.ProductionId,
                ProductId = p.ProductId,
                ProductName = p.Product.Name,
                Quantity = p.Quantity,
                Status = p.Status.ToString(),
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                CreatedByUserName = p.User.UserName,
                OrderId = p.OrderId
            });

            var totalRecords = await dtoQuery.CountAsync();
            var items = await dtoQuery.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
            var pagedResponse = new PagedResponse<ProductionOrderDto>(items, filters.PageNumber, filters.PageSize, totalRecords);

            return ResponseHelper<PagedResponse<ProductionOrderDto>>.Ok(pagedResponse);
        }

        public async Task<ResponseHelper<ProductionOrderDto>> GetPrdouctionOrderByIdAsync(int id)
        {
            var productionOrder = await _context.ProductionOrders
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProductionId == id);

            if (productionOrder == null) return ResponseHelper<ProductionOrderDto>.Fail("Üretim emri bulunamadı.");

            var dto = _mapper.Map<ProductionOrderDto>(productionOrder);
            return ResponseHelper<ProductionOrderDto>.Ok(dto);
        }

        public async Task<ResponseHelper<ProductionOrder>> UpdatePrdouctionOrderAsync(ProductionOrder updatedProOrder, int performingUserId)
        {
            var existingOrder = await _context.ProductionOrders.FindAsync(updatedProOrder.ProductionId);
            if (existingOrder == null) return ResponseHelper<ProductionOrder>.Fail("Güncellenecek üretim emri bulunamadı.");

            var changes = new Dictionary<string, object>();
            // DÜZELTME: ProductName yerine ProductId karşılaştırılıyor
            if (existingOrder.ProductId != updatedProOrder.ProductId) changes["ProductId"] = new { Old = existingOrder.ProductId, New = updatedProOrder.ProductId };
            if (existingOrder.Quantity != updatedProOrder.Quantity) changes["Quantity"] = new { Old = existingOrder.Quantity, New = updatedProOrder.Quantity };
            if (existingOrder.Status != updatedProOrder.Status) changes["Status"] = new { Old = existingOrder.Status.ToString(), New = updatedProOrder.Status.ToString() };

            existingOrder.ProductId = updatedProOrder.ProductId;
            existingOrder.Quantity = updatedProOrder.Quantity;
            existingOrder.Status = updatedProOrder.Status;

            await _context.SaveChangesAsync();

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
    }
}