using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Helpers
{
    public class StockTransactionFilterParameters : LogFilterParameters
    {
        public TransactionType? TransactionType { get; set; }
        public string? ProductName { get; set; }
    }
}
