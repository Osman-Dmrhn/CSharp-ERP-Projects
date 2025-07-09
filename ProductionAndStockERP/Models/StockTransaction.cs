namespace ProductionAndStockERP.Models
{
    public class StockTransaction
    {
        public int StockTxnId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public int? RelatedOrderId { get; set; }
        public Order Order { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public enum TransactionType
    {
        Entry,
        Exit
    }
}
