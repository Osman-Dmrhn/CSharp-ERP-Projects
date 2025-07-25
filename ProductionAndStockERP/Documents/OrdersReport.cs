using ProductionAndStockERP.Dtos.OrderDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace ProductionAndStockERP.Documents
{
    public class OrdersReport : IDocument
    {
        private readonly IEnumerable<OrderDto> _orders;

        public OrdersReport(IEnumerable<OrderDto> orders)
        {
            _orders = orders;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber().FontSize(10);
                    text.Span(" / ").FontSize(10);
                    text.TotalPages().FontSize(10);
                });
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Sipariş Raporu")
                        .Bold().FontSize(24).FontColor(Colors.Blue.Medium);

                    row.ConstantItem(150).AlignRight().Text(text =>
                    {
                        text.Span("Rapor Tarihi: ").SemiBold();
                        text.Span($"{DateTime.Now:dd.MM.yyyy}");
                    });
                });

                column.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Spacing(25);

                if (!_orders.Any())
                {
                    column.Item().Text("Raporlanacak sipariş bulunamadı.").FontSize(14);
                    return;
                }

                column.Item().Element(ComposeTable);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);  // #ID
                    columns.RelativeColumn(3);   // Müşteri Adı
                    columns.RelativeColumn(4);   // Ürün Adı
                    columns.ConstantColumn(50);  // Miktar
                    columns.RelativeColumn(2);   // Durum
                    columns.RelativeColumn(3);   // Oluşturan
                    columns.RelativeColumn(3);   // Tarih
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("#ID").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Müşteri").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Ürün").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Miktar").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Durum").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Oluşturan").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Tarih").FontColor(Colors.White);
                });

                foreach (var (order, index) in _orders.Select((value, i) => (value, i)))
                {
                    var backgroundColor = index % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Background(backgroundColor).Padding(5).Text(order.OrderId);
                    table.Cell().Background(backgroundColor).Padding(5).Text(order.CustomerName);
                    table.Cell().Background(backgroundColor).Padding(5).Text(order.ProductName);
                    table.Cell().Background(backgroundColor).Padding(5).AlignCenter().Text(order.Quantity);
                    table.Cell().Background(backgroundColor).Padding(5).Text(order.Status);
                    table.Cell().Background(backgroundColor).Padding(5).Text(order.UserName);
                    table.Cell().Background(backgroundColor).Padding(5).Text(order.CreatedAt.ToString("dd.MM.yyyy HH:mm"));
                }
            });
        }
    }
}