using ProductionAndStockERP.Dtos.ProductDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace ProductionAndStockERP.Documents
{
    public class ProductsReport : IDocument
    {
        private readonly IEnumerable<ProductDto> _products;

        public ProductsReport(IEnumerable<ProductDto> products)
        {
            _products = products;
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

                    row.RelativeItem().Text("Ürün Raporu")
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

                if (!_products.Any())
                {
                    column.Item().Text("Raporlanacak ürün bulunamadı.").FontSize(14);
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
                    columns.ConstantColumn(40);  
                    columns.RelativeColumn(3);   
                    columns.RelativeColumn(5);   
                    columns.RelativeColumn(2);   
                });

                table.Header(header =>
                {

                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("#ID").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Ürün Adı").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Açıklama").FontColor(Colors.White);
                    header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Eklenme Tarihi").FontColor(Colors.White);
                });

                foreach (var (product, index) in _products.Select((value, i) => (value, i)))
                {

                    var cellStyle = TextStyle.Default.FontSize(10);


                    var backgroundColor = index % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Background(backgroundColor).Padding(5).Text(product.ProductId).Style(cellStyle);
                    table.Cell().Background(backgroundColor).Padding(5).Text(product.Name).Style(cellStyle);
                    table.Cell().Background(backgroundColor).Padding(5).Text(product.Description ?? "-").Style(cellStyle);
                    table.Cell().Background(backgroundColor).Padding(5).Text(product.CreatedAt.ToString("dd.MM.yyyy")).Style(cellStyle);
                }
            });
        }
    }
}