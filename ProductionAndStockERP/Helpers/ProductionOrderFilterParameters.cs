namespace ProductionAndStockERP.Helpers
{
    public class ProductionOrderFilterParameters: LogFilterParameters
    {
        public int? ProductId { get; set; }
        public string? Status { get; set; }
    }
}
