using Microsoft.EntityFrameworkCore;

namespace Crosscutting.Persistence.Repositories.Implementation;

public static class IQueryableExtensions
{
    public static async Task<Paginate<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        int page,
        int pageSize,
        CancellationToken token = default) where TEntity : class, new()
    {
        var count = await source.CountAsync(token);
        if (count > 0)
        {
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);
            return new Paginate<TEntity>(items, count, page, pageSize);
        }

        return new([], 0, 0, 0);
    }
}
