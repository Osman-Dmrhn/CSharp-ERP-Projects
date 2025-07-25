using ProductionAndStockERP.Dtos.ProductionOrderDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProductionAndStockERP.Documents
{
    public class ProductionOrdersReport : IDocument
    {
        private readonly IEnumerable<ProductionOrderDto> _productionOrders;
        public ProductionOrdersReport(IEnumerable<ProductionOrderDto> productionOrders)
        {
            _productionOrders = productionOrders;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                // Başlık
                page.Header()
                    .AlignCenter()
                    .Text("Üretim Emri Raporu")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                // İçerik (Tablo)
                page.Content().PaddingVertical(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);  // ID
                        columns.RelativeColumn(4);   // Ürün Adı
                        columns.ConstantColumn(50);  // Miktar
                        columns.RelativeColumn(2);   // Durum
                        columns.RelativeColumn(3);   // Oluşturan
                        columns.RelativeColumn(3);   // Tarih
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("#ID").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Ürün Adı").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Miktar").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Durum").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Oluşturan").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Tarih").FontColor(Colors.White);
                    });

                    foreach (var order in _productionOrders)
                    {
                        table.Cell().Padding(2).Text(order.ProductionId);
                        table.Cell().Padding(2).Text(order.ProductName);
                        table.Cell().Padding(2).AlignCenter().Text(order.Quantity);
                        table.Cell().Padding(2).Text(order.Status);
                        table.Cell().Padding(2).Text(order.CreatedByUserName);
                        table.Cell().Padding(2).Text(order.CreatedAt.ToString("dd.MM.yyyy HH:mm"));
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        }
    }
}