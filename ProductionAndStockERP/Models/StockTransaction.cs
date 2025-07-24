using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Models
{
    public class StockTransaction
    {
        public int StockTxnId { get; set; }
        
        
        [Required]
        public int ProductId { get; set; } 
        public Product Product { get; set; }
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
