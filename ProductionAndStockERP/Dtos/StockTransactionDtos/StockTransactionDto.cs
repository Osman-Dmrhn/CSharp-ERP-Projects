using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Dtos.StockTransactionDtos
{
    public class StockTransactionDto
    {
        public int StockTxnId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public int Quantity { get; set; }
        public string TransactionType { get; set; }
        public int? RelatedOrderId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
