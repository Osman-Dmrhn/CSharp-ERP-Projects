using ProductionAndStockERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.ProductionOrder
{
    public class ProductionOrderCreateDto
    {
        
        public int? OrderId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }

    }
}
