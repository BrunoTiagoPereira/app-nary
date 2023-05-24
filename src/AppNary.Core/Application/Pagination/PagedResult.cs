namespace AppNary.Core.Application.Pagination
{
    public sealed class PagedResult<T> : PagedResult where T : class
    {
        public IEnumerable<T> Items { get; set; }
    }

    public abstract class PagedResult
    {
        public const int DEFAULT_PAGE_SIZE = 8;

        public const int DEFAULT_PAGE_INDEX = 1;

        public const string DEFAULT_QUERY = null;

        public int TotalResults { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
    }
}
