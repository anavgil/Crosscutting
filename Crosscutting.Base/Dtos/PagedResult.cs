namespace Crosscutting.Application.Dtos;

public interface IPagedResult<T> where T : class?
{
    int PageNumber { get; set; }

    int TotalPages { get; set; }

    bool IsFirstPage { get; set; }

    bool IsLastPage { get; set; }

    int Count { get; set; }

    IReadOnlyCollection<T?> Items { get; set; }
}

public class PagedResult<T> : IPagedResult<T> where T : class?
{
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public bool IsFirstPage { get; set; }
    public bool IsLastPage { get; set; }
    public int Count { get; set; }
    public IReadOnlyCollection<T?> Items { get; set; } = [];
}
