using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public OrderStatus Status { get; set; }

        public int UserId { get; set; }
        public User CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        InProduction, 
        Completed, 
        Cancelled 
    }
}
