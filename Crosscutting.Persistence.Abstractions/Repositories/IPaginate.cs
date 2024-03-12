namespace Crosscutting.Persistence.Abstractions.Repositories;

public interface IPaginate<TEntity> where TEntity : class, new()
{
    int From { get; }

    int Index { get; }

    int Size { get; }

    int Count { get; }

    int Pages { get; }

    IList<TEntity> Items { get; }

    bool HasPrevious { get; }

    bool HasNext { get; }
}
