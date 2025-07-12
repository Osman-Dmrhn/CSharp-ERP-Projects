using ProductionAndStockERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.OrderDtos
{
    public class OrderUpdateDto
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public OrderStatus Status { get; set; }
    }
}
