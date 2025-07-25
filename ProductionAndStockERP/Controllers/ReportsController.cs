using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Documents;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using QuestPDF.Fluent;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }


        [HttpGet("orders")]
        [Authorize(Roles = "Admin,SalesManager")] 
        public async Task<IActionResult> GetOrdersReport([FromQuery] OrderFilterParameters filters)
        {
            var data = await _reportService.GetFilteredOrdersForReportAsync(filters);
            if (!data.Any())
                return NotFound("Filtrelere uygun raporlanacak sipariş bulunamadı.");

            var reportDocument = new OrdersReport(data);
            byte[] pdfBytes = reportDocument.GeneratePdf();
            string fileName = $"Siparis_Raporu_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        [HttpGet("products")]
        [Authorize(Roles = "Admin,Producer")] 
        public async Task<IActionResult> GetProductsReport([FromQuery] ProductFilterParameters filters)
        {
            var data = await _reportService.GetFilteredProductsForReportAsync(filters);
            if (!data.Any())
                return NotFound("Filtrelere uygun raporlanacak ürün bulunamadı.");

            var reportDocument = new ProductsReport(data);
            byte[] pdfBytes = reportDocument.GeneratePdf();
            string fileName = $"Urun_Raporu_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        [HttpGet("production-orders")]
        [Authorize(Roles = "Admin,Producer")]
        public async Task<IActionResult> GetProductionOrdersReport([FromQuery] ProductionOrderFilterParameters filters)
        {
            var data = await _reportService.GetFilteredProductionOrdersForReportAsync(filters);
            if (!data.Any())
                return NotFound("Filtrelere uygun raporlanacak üretim emri bulunamadı.");

            var reportDocument = new ProductionOrdersReport(data); 
            byte[] pdfBytes = reportDocument.GeneratePdf();
            string fileName = $"Uretim_Emri_Raporu_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        // --- STOK HAREKETİ RAPORU ---
        [HttpGet("stock-transactions")]
        [Authorize(Roles = "Admin,Producer")]
        public async Task<IActionResult> GetStockTransactionsReport([FromQuery] StockTransactionFilterParameters filters)
        {
            var data = await _reportService.GetFilteredStockTransactionsForReportAsync(filters);
            if (!data.Any())
                return NotFound("Filtrelere uygun raporlanacak stok hareketi bulunamadı.");

            var reportDocument = new StockTransactionsReport(data); // Bu sınıfı oluşturmanız gerekiyor
            byte[] pdfBytes = reportDocument.GeneratePdf();
            string fileName = $"Stok_Hareketi_Raporu_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        // --- AKTİVİTE LOG RAPORU ---
        [HttpGet("logs")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLogsReport([FromQuery] LogFilterParameters filters)
        {
            var data = await _reportService.GetFilteredLogsForReportAsync(filters);
            if (!data.Any())
                return NotFound("Filtrelere uygun raporlanacak log kaydı bulunamadı.");

            var reportDocument = new LogsReport(data); // Bu sınıfı oluşturmanız gerekiyor
            byte[] pdfBytes = reportDocument.GeneratePdf();
            string fileName = $"Aktivite_Log_Raporu_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}