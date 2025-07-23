using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Dtos.StockTransactionDtos
{
    public class StockTransactionDto
    {
        public int StockTxnId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
