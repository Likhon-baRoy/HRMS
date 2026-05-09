namespace API.Responses;

public class PaginationMeta
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasNextPage => Page < TotalPages;

    public bool HasPreviousPage => Page > 1;

    public PaginationMeta() { }

    public PaginationMeta(int page, int pageSize, int totalCount)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;

        TotalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize
        );
    }
}
