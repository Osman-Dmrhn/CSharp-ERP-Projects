using ProductionAndStockERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.ProductionOrder
{
    public class ProductionOrderUpdateDto
    {
        [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
        public int Quantity { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
