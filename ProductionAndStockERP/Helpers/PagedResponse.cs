namespace ProductionAndStockERP.Helpers
{
    public class PagedResponse<T> where T : class
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public List<T> Items { get; private set; }

        public PagedResponse(List<T> items, int pageNumber, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Items = items;
        }
    }
}