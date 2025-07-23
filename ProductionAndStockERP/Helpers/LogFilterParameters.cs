namespace ProductionAndStockERP.Helpers
{
    public class LogFilterParameters
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? UserName { get; set; }
        public string? Status { get; set; }
        public string? Action { get; set; }

        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }
    }
}