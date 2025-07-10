using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Services
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly ApplicationDbContext _context;
        public StockTransactionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseHelper<bool>> CreateStockTransactionAsync(StockTransaction stockT)
        {
            _context.StockTransactions.Add(stockT);
            await _context.SaveChangesAsync();
            return ResponseHelper<bool>.Ok(true);
        }

        public async Task<ResponseHelper<bool>> DeleteStockTransactionAsync(int id)
        {
            var result = await _context.StockTransactions.FindAsync(id);
            if(result == null) return ResponseHelper<bool>.Fail("StockTransaction Bulunamadı");
            _context.StockTransactions.Remove(result);
            _context.SaveChanges();
            return ResponseHelper<bool>.Ok(true);
        }

        public async Task<ResponseHelper<IEnumerable<StockTransaction>>> GetAllStockTransactionAsync()
        {
            var result = await _context.StockTransactions.ToListAsync();
            return ResponseHelper<IEnumerable<StockTransaction>>.Ok(result);
        }

        public async Task<ResponseHelper<StockTransaction>> GetStockTransactionByIdAsync(int id)
        {
            var result = await _context.StockTransactions.FindAsync(id);
            if (result == null) return ResponseHelper<StockTransaction>.Fail("StockTransaction Bulunamadı");
            return ResponseHelper<StockTransaction>.Ok(result);
        }

        public async Task<ResponseHelper<bool>> UpdateStockTransactionAsync(StockTransaction stockT)
        {
            var result = await _context.StockTransactions.FindAsync(stockT.StockTxnId);
            if (result == null) return ResponseHelper<bool>.Fail("StockTransaction Bulunamadı");

            stockT.Quantity = result.Quantity;
            stockT.TransactionType = stockT.TransactionType;

            _context.StockTransactions.Update(stockT);
            await _context.SaveChangesAsync();

            return ResponseHelper<bool>.Ok(true);
        }
    }
}
