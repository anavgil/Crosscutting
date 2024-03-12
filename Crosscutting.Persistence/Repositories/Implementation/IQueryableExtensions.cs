using Crosscutting.Persistence.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Crosscutting.Persistence.Repositories.Implementation;

public static class IQueryableExtensions
{
    public static async Task<IPaginate<TEntity>> ToPaginateAsync<TEntity>(this IQueryable<TEntity> source,
                                                                    int index, int size, int from = 0,
                                                                    CancellationToken cancellationToken = default)
        where TEntity : class, new()
    {
        if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must From <= Index");

        var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

        var items = await source.Skip((index - from) * size)
                                .Take(size)
                                .ToListAsync(cancellationToken).ConfigureAwait(false);

        var result = new Paginate<TEntity>();

        result.Index = index;
        result.Count = count;
        result.Items = items;
        result.From = from;
        result.Pages = (int)(Math.Ceiling(count / (double)size));

        return result;
    }
}
