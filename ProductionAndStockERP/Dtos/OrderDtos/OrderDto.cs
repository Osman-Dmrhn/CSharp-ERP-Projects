namespace ProductionAndStockERP.Dtos.OrderDtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } 
    }
}
