namespace ProductionAndStockERP.Dtos.ProductionOrderDtos
{
    public class ProductionOrderDto
    {
        public int ProductionId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public int? OrderId { get; set; }
    }
}
