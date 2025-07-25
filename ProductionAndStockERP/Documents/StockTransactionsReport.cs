using ProductionAndStockERP.Dtos.StockTransactionDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProductionAndStockERP.Documents
{
    public class StockTransactionsReport : IDocument
    {
        private readonly IEnumerable<StockTransactionDto> _transactions;
        public StockTransactionsReport(IEnumerable<StockTransactionDto> transactions)
        {
            _transactions = transactions;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header()
                    .AlignCenter()
                    .Text("Stok Hareketleri Raporu")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content().PaddingVertical(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);  // ID
                        columns.RelativeColumn(5);   // Ürün Adı
                        columns.RelativeColumn(2);   // İşlem Tipi
                        columns.ConstantColumn(50);  // Miktar
                        columns.RelativeColumn(3);   // Tarih
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("#ID").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Ürün Adı").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("İşlem Tipi").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Miktar").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Tarih").FontColor(Colors.White);
                    });

                    foreach (var txn in _transactions)
                    {
                        // İşlem tipine göre renklendirme
                        var transactionTypeColor = txn.TransactionType == "Entry" ? Colors.Green.Medium : Colors.Red.Medium;

                        table.Cell().Padding(2).Text(txn.StockTxnId);
                        table.Cell().Padding(2).Text(txn.ProductName);
                        table.Cell().Padding(2).Text(txn.TransactionType == "Entry" ? "Giriş" : "Çıkış").FontColor(transactionTypeColor);
                        table.Cell().Padding(2).AlignCenter().Text(txn.Quantity);
                        table.Cell().Padding(2).Text(txn.CreatedAt.ToString("dd.MM.yyyy HH:mm"));
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