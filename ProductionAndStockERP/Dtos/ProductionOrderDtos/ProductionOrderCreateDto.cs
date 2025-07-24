using ProductionAndStockERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.ProductionOrder
{
    public class ProductionOrderCreateDto
    {
        [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
        public int Quantity { get; set; }

        public int? OrderId { get; set; }

    }
}
