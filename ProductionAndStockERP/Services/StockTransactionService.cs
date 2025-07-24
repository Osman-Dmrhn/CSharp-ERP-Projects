// Dosya: Services/StockTransactionService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService;
        private readonly IMapper _mapper;

        public StockTransactionService(ApplicationDbContext context, IActivityLogsService activityLogsService, IMapper mapper)
        {
            _context = context;
            _activityLogsService = activityLogsService;
            _mapper = mapper;
        }

        public async Task<ResponseHelper<PagedResponse<StockTransactionDto>>> GetAllStockTransactionAsync(StockTransactionFilterParameters filters)
        {
            var query = _context.StockTransactions
                .Include(st => st.Product) // Product bilgilerini çek
                .AsQueryable();

            // Filtreleme
            if (filters.ProductId.HasValue)
                query = query.Where(st => st.ProductId == filters.ProductId.Value);
            if (!string.IsNullOrEmpty(filters.TransactionType))
                query = query.Where(st => st.TransactionType.ToString() == filters.TransactionType);

            query = query.OrderByDescending(st => st.CreatedAt);

            var dtoQuery = query.Select(st => new StockTransactionDto
            {
                StockTxnId = st.StockTxnId,
                ProductId = st.ProductId,
                ProductName = st.Product.Name,
                Quantity = st.Quantity,
                TransactionType = st.TransactionType.ToString(),
                RelatedOrderId = st.RelatedOrderId,
                CreatedAt = st.CreatedAt
            });

            var totalRecords = await dtoQuery.CountAsync();
            var items = await dtoQuery.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
            var pagedResponse = new PagedResponse<StockTransactionDto>(items, filters.PageNumber, filters.PageSize, totalRecords);

            return ResponseHelper<PagedResponse<StockTransactionDto>>.Ok(pagedResponse);
        }

        public async Task<ResponseHelper<StockTransactionDto>> GetStockTransactionByIdAsync(int id)
        {
            var transaction = await _context.StockTransactions
                .Include(st => st.Product)
                .FirstOrDefaultAsync(st => st.StockTxnId == id);

            if (transaction == null) return ResponseHelper<StockTransactionDto>.Fail("Stok hareketi bulunamadı.");

            var dto = _mapper.Map<StockTransactionDto>(transaction);
            return ResponseHelper<StockTransactionDto>.Ok(dto);
        }

        public async Task<ResponseHelper<StockTransaction>> CreateStockTransactionAsync(StockTransaction stockT, int performingUserId)
        {
            await _context.StockTransactions.AddAsync(stockT);
            await _context.SaveChangesAsync();

            await _activityLogsService.AddLogAsync(performingUserId, "Yeni stok hareketi oluşturuldu.", "Başarılı", "StockTransaction", stockT.StockTxnId.ToString());

            return ResponseHelper<StockTransaction>.Ok(stockT);
        }

        public async Task<ResponseHelper<StockTransaction>> UpdateStockTransactionAsync(StockTransaction updatedStockT, int performingUserId)
        {
            var existingTransaction = await _context.StockTransactions.FindAsync(updatedStockT.StockTxnId);
            if (existingTransaction == null) return ResponseHelper<StockTransaction>.Fail("Güncellenecek stok hareketi bulunamadı.");

            var changes = new Dictionary<string, object>();
            if (existingTransaction.ProductId != updatedStockT.ProductId) changes["ProductId"] = new { Old = existingTransaction.ProductId, New = updatedStockT.ProductId };
            if (existingTransaction.Quantity != updatedStockT.Quantity) changes["Quantity"] = new { Old = existingTransaction.Quantity, New = updatedStockT.Quantity };
            if (existingTransaction.TransactionType != updatedStockT.TransactionType) changes["TransactionType"] = new { Old = existingTransaction.TransactionType.ToString(), New = updatedStockT.TransactionType.ToString() };

            existingTransaction.ProductId = updatedStockT.ProductId;
            existingTransaction.Quantity = updatedStockT.Quantity;
            existingTransaction.TransactionType = updatedStockT.TransactionType;
            existingTransaction.RelatedOrderId = updatedStockT.RelatedOrderId;

            await _context.SaveChangesAsync();

            string changesJson = changes.Count > 0 ? JsonSerializer.Serialize(changes) : null;
            await _activityLogsService.AddLogAsync(performingUserId, "Stok hareketi güncellendi.", "Başarılı", "StockTransaction", existingTransaction.StockTxnId.ToString(), changesJson);

            return ResponseHelper<StockTransaction>.Ok(existingTransaction);
        }

        public async Task<ResponseHelper<bool>> DeleteStockTransactionAsync(int id, int performingUserId)
        {
            var transaction = await _context.StockTransactions.FindAsync(id);
            if (transaction == null)
            {
                await _activityLogsService.AddLogAsync(performingUserId, $"ID'si {id} olan stok hareketini silme denemesi başarısız.", "Başarısız", "StockTransaction", id.ToString());
                return ResponseHelper<bool>.Fail("Stok hareketi bulunamadı.");
            }

            _context.StockTransactions.Remove(transaction);
            await _context.SaveChangesAsync();

            await _activityLogsService.AddLogAsync(performingUserId, "Stok hareketi silindi.", "Başarılı", "StockTransaction", id.ToString());
            return ResponseHelper<bool>.Ok(true);
        }
    }
}