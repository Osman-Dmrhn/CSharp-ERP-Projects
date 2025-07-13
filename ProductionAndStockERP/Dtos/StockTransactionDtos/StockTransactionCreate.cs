using ProductionAndStockERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.StockTransactionDtos
{
    public class StockTransactionCreate
    {
        [Required]
        public string ProductName { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }
        public int? RelatedOrderId { get; set; }
    }
}
