using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Models
{
    public class ProductionOrder
    {
        public int ProductionId { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; } 
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Status Status { get; set; }

        public int CreatedBy { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public enum Status
    {
        Completed,
        InProgress,
        Started
    }
}
