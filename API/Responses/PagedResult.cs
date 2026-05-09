namespace API.Responses;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public PaginationMeta Meta { get; set; } = new();

    public PagedResult() { }

    public PagedResult(IEnumerable<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;

        Meta = new PaginationMeta(
            page,
            pageSize,
            totalCount
        );
    }
}
