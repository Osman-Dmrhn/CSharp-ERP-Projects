namespace ProductionAndStockERP.Helpers
{
    public class OrderFilterParameters:LogFilterParameters
    {
        public int? ProductId { get; set; }
        public string? CustomerName { get; set; }
        public string? Status { get; set; }
    }
}
