namespace API.Responses;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public PaginationMeta Meta { get; set; } = new();
}