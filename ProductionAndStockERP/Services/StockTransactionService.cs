// Dosya: Services/StockTransactionService.cs
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService;

        public StockTransactionService(ApplicationDbContext context, IActivityLogsService activityLogsService)
        {
            _context = context;
            _activityLogsService = activityLogsService;
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
            if (existingTransaction == null)
            {
                return ResponseHelper<StockTransaction>.Fail("Güncellenecek stok hareketi bulunamadı.");
            }

            var changes = new Dictionary<string, object>();
            if (existingTransaction.Quantity != updatedStockT.Quantity)
                changes["Quantity"] = new { Old = existingTransaction.Quantity, New = updatedStockT.Quantity };
            if (existingTransaction.TransactionType != updatedStockT.TransactionType)
                changes["TransactionType"] = new { Old = existingTransaction.TransactionType, New = updatedStockT.TransactionType };

            existingTransaction.Quantity = updatedStockT.Quantity;
            existingTransaction.TransactionType = updatedStockT.TransactionType;

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
            await _context.SaveChangesAsync(); // DÜZELTME: SaveChanges() -> SaveChangesAsync()

            await _activityLogsService.AddLogAsync(performingUserId, "Stok hareketi silindi.", "Başarılı", "StockTransaction", id.ToString());

            return ResponseHelper<bool>.Ok(true);
        }

        public async Task<ResponseHelper<PagedResponse<StockTransactionDto>>> GetAllStockTransactionAsync(StockTransactionFilterParameters filters)
        {
            var query = _context.StockTransactions.AsQueryable();
            // Filtreleme ve sayfalama mantığı buraya eklenebilir (GetAllLogsAsync gibi)

            var dtoQuery = query.OrderByDescending(x => x.CreatedAt).Select(x => new StockTransactionDto
            {
                StockTxnId = x.StockTxnId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                TransactionType = x.TransactionType,
                CreatedAt = x.CreatedAt
            });

            var totalRecords = await dtoQuery.CountAsync();
            var items = await dtoQuery.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
            var pagedResponse = new PagedResponse<StockTransactionDto>(items, filters.PageNumber, filters.PageSize, totalRecords);

            return ResponseHelper<PagedResponse<StockTransactionDto>>.Ok(pagedResponse);
        }

        public async Task<ResponseHelper<StockTransaction>> GetStockTransactionByIdAsync(int id)
        {
            var result = await _context.StockTransactions.FindAsync(id);
            if (result == null) return ResponseHelper<StockTransaction>.Fail("Stok hareketi bulunamadı.");
            return ResponseHelper<StockTransaction>.Ok(result);
        }
    }
}