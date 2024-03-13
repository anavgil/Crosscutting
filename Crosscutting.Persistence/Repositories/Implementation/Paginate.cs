using Crosscutting.Persistence.Abstractions.Repositories;
using System.Collections;

namespace Crosscutting.Persistence.Repositories.Implementation;

public class Paginate<TEntity> : IReadOnlyList<TEntity>,
                                IPaginate<TEntity> where TEntity : class, new()
{
    private readonly IList<TEntity> subset;

    public Paginate(IEnumerable<TEntity> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        subset = items as IList<TEntity> ?? new List<TEntity>(items);
    }

    public int PageNumber { get; }
    public int TotalPages { get; }

    public TEntity this[int index] => subset[index];

    public bool IsFirstPage => PageNumber == 1;

    public bool IsLastPage => PageNumber == TotalPages;

    public int Count => subset.Count;

    public IReadOnlyList<TEntity> Data => [.. subset];

    public IEnumerator<TEntity> GetEnumerator() => subset.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => subset.GetEnumerator();
}
