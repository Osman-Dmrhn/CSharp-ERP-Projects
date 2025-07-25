using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.ActivityLogDtos;
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Dtos.ProductDtos;
using ProductionAndStockERP.Dtos.ProductionOrderDtos;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using System.Linq.Dynamic.Core; // Sıralama için bu using ifadesi gerekebilir.

namespace ProductionAndStockERP.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetFilteredOrdersForReportAsync(OrderFilterParameters filters)
        {
            var query = _context.Orders
                .Include(o => o.Product)
                .Include(o => o.CreatedBy) // Modelinizdeki property adı 'User' olmalı.
                .AsQueryable();

            if (filters.ProductId.HasValue)
                query = query.Where(o => o.ProductId == filters.ProductId.Value);
            if (!string.IsNullOrEmpty(filters.CustomerName))
                query = query.Where(o => o.CustomerName.Contains(filters.CustomerName));
            if (!string.IsNullOrEmpty(filters.Status))
                query = query.Where(o => o.Status.ToString() == filters.Status);

            query = query.OrderByDescending(o => o.CreatedAt);

            var results = await query.Select(o => new OrderDto
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
            }).ToListAsync();

            return results;
        }

        public async Task<IEnumerable<ProductionOrderDto>> GetFilteredProductionOrdersForReportAsync(ProductionOrderFilterParameters filters)
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

            return await query.Select(p => new ProductionOrderDto
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
            }).ToListAsync();
        }

        public async Task<IEnumerable<StockTransactionDto>> GetFilteredStockTransactionsForReportAsync(StockTransactionFilterParameters filters)
        {
            var query = _context.StockTransactions
                .Include(st => st.Product)
                .AsQueryable();

            if (filters.ProductId.HasValue)
                query = query.Where(st => st.ProductId == filters.ProductId.Value);
            if (!string.IsNullOrEmpty(filters.TransactionType))
                query = query.Where(st => st.TransactionType.ToString() == filters.TransactionType);

            query = query.OrderByDescending(st => st.CreatedAt);

            return await query.Select(st => new StockTransactionDto
            {
                StockTxnId = st.StockTxnId,
                ProductId = st.ProductId,
                ProductName = st.Product.Name,
                Quantity = st.Quantity,
                TransactionType = st.TransactionType.ToString(),
                RelatedOrderId = st.RelatedOrderId,
                CreatedAt = st.CreatedAt
            }).ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredProductsForReportAsync(ProductFilterParameters filters)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(p => p.Name.Contains(filters.Name));

            query = query.OrderBy(p => p.Name);

            return await query.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            }).ToListAsync();
        }

        public async Task<IEnumerable<ActivityLogDto>> GetFilteredLogsForReportAsync(LogFilterParameters filters)
        {
            var query = _context.ActivityLogs
                .Include(log => log.User)
                .AsQueryable();

            if (filters.StartDate.HasValue)
                query = query.Where(l => l.CreatedAt >= filters.StartDate.Value);
            if (filters.EndDate.HasValue)
                query = query.Where(l => l.CreatedAt <= filters.EndDate.Value);
            if (!string.IsNullOrEmpty(filters.UserName))
                query = query.Where(l => l.User.UserName.Contains(filters.UserName));
            if (!string.IsNullOrEmpty(filters.Status))
                query = query.Where(l => l.Status == filters.Status);

            query = query.OrderByDescending(l => l.CreatedAt);

            return await query.Select(log => new ActivityLogDto
            {
                Id = log.LogId,
                UserName = log.User.UserName,
                Action = log.Action,
                CreatedAt = log.CreatedAt,
                Status = log.Status,
                TargetEntity = log.TargetEntity
            }).ToListAsync();
        }
    }
}