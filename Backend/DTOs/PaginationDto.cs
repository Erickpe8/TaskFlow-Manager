namespace TaskFlow.Api.DTOs
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }

    public class TaskFilterDto
    {
        public string? Search { get; set; }
        public int? ColumnId { get; set; }
        public int? Priority { get; set; }
        public string? SortBy { get; set; } = "title";
        public bool SortDesc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}