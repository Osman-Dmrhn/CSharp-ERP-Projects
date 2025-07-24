// Dosya: Services/OrderService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IActivityLogsService activityLogsService,IMapper mapper)
        {
            _context = context;
            _activityLogsService = activityLogsService;
            _mapper = mapper;
        }

        public async Task<ResponseHelper<Order>> CreateOrderAsync(Order order, int performingUserId)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            await _activityLogsService.AddLogAsync(performingUserId, "Yeni sipariş oluşturuldu.", "Başarılı", "Order", order.OrderId.ToString());

            return ResponseHelper<Order>.Ok(order);
        }
        public async Task<ResponseHelper<PagedResponse<OrderDto>>> GetAllOrdersAsync(OrderFilterParameters filters)
        {
            var query = _context.Orders
                .Include(o => o.Product)
                .Include(o => o.CreatedBy)     
                .AsQueryable();

            // Filtreleme
            if (filters.ProductId.HasValue)
                query = query.Where(o => o.ProductId == filters.ProductId.Value);
            if (!string.IsNullOrEmpty(filters.CustomerName))
                query = query.Where(o => o.CustomerName.Contains(filters.CustomerName));
            if (!string.IsNullOrEmpty(filters.Status))
                query = query.Where(o => o.Status.ToString() == filters.Status);

            query = query.OrderByDescending(o => o.CreatedAt);

            var dtoQuery = query.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                Quantity = o.Quantity,
                Status = o.Status.ToString(),
                CustomerName = o.CustomerName,
                CreatedAt = o.CreatedAt,
                UserId = o.UserId,
                UserName = o.CreatedBy.UserName
            });

            var totalRecords = await dtoQuery.CountAsync();
            var items = await dtoQuery.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
            var pagedResponse = new PagedResponse<OrderDto>(items, filters.PageNumber, filters.PageSize, totalRecords);

            return ResponseHelper<PagedResponse<OrderDto>>.Ok(pagedResponse);
        }

        public async Task<ResponseHelper<OrderDto>> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.CreatedBy)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return ResponseHelper<OrderDto>.Fail("Sipariş Bulunamadı");

            var orderDto = _mapper.Map<OrderDto>(order);
            return ResponseHelper<OrderDto>.Ok(orderDto);
        }

        public async Task<ResponseHelper<Order>> UpdateOrderAsync(Order updatedOrder, int performingUserId)
        {
            var existingOrder = await _context.Orders.FindAsync(updatedOrder.OrderId);
            if (existingOrder == null) return ResponseHelper<Order>.Fail("Güncellenecek sipariş bulunamadı.");

            var changes = new Dictionary<string, object>();

            if (existingOrder.ProductId != updatedOrder.ProductId) changes["ProductId"] = new { Old = existingOrder.ProductId, New = updatedOrder.ProductId };
            if (existingOrder.Quantity != updatedOrder.Quantity) changes["Quantity"] = new { Old = existingOrder.Quantity, New = updatedOrder.Quantity };
            if (existingOrder.Status != updatedOrder.Status) changes["Status"] = new { Old = existingOrder.Status.ToString(), New = updatedOrder.Status.ToString() };
            if (existingOrder.CustomerName != updatedOrder.CustomerName) changes["CustomerName"] = new { Old = existingOrder.CustomerName, New = updatedOrder.CustomerName };


            existingOrder.ProductId = updatedOrder.ProductId;
            existingOrder.Quantity = updatedOrder.Quantity;
            existingOrder.Status = updatedOrder.Status;
            existingOrder.CustomerName = updatedOrder.CustomerName;

            await _context.SaveChangesAsync();

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

    }
}