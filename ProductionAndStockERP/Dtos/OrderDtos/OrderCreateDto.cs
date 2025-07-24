using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.OrderDtos
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
        public int Quantity { get; set; }

        [Required]
        public string CustomerName { get; set; }
    }
}
