using ProductionAndStockERP.Dtos.ActivityLogDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProductionAndStockERP.Documents
{
    public class LogsReport : IDocument
    {
        private readonly IEnumerable<ActivityLogDto> _logs;
        public LogsReport(IEnumerable<ActivityLogDto> logs)
        {
            _logs = logs;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header()
                    .AlignCenter()
                    .Text("Aktivite Log Raporu")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content().PaddingVertical(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);   // Kullanıcı
                        columns.RelativeColumn(6);   // Eylem
                        columns.RelativeColumn(2);   // Durum
                        columns.RelativeColumn(3);   // Tarih
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Kullanıcı").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Eylem").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Durum").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Tarih").FontColor(Colors.White);
                    });

                    foreach (var log in _logs)
                    {
                        table.Cell().Padding(2).Text(log.UserName);
                        table.Cell().Padding(2).Text(log.Action);
                        table.Cell().Padding(2).Text(log.Status);
                        table.Cell().Padding(2).Text(log.CreatedAt.ToString("dd.MM.yyyy HH:mm:ss"));
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