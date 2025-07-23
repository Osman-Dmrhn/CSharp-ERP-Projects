using ProductionAndStockERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductionAndStockERP.Dtos.StockTransactionDtos
{
    public class StockTransactionUpdateDto
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2 ile 100 karakter arasında olmalıdır.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "İşlem tipi zorunludur.")]
        public TransactionType TransactionType { get; set; }

        public int? RelatedOrderId { get; set; }
    }
}