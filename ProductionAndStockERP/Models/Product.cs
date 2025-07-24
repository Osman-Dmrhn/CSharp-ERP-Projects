using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Ürün adı 150 karakterden uzun olamaz.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Açıklama 500 karakterden uzun olamaz.")]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
