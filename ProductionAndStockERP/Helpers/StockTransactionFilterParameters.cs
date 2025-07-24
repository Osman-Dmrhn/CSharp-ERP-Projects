using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Helpers
{
    public class StockTransactionFilterParameters : LogFilterParameters
    {
        public int? ProductId { get; set; }
        public string? TransactionType { get; set; }
    }
}
