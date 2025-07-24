using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.ProductDtos
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
