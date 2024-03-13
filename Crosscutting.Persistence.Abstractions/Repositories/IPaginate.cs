namespace Crosscutting.Persistence.Abstractions.Repositories;

public interface IPaginate<TEntity> where TEntity : class, new()
{
    int PageNumber { get; }

    int TotalPages { get; }

    bool IsFirstPage => PageNumber == 1;

    bool IsLastPage => PageNumber == TotalPages;

    int Count { get; }


}
