using System.Linq.Expressions;

namespace Crosscutting.Persistence.Abstractions.Specifications;

public class ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    Expression<Func<TEntity, object>> OrderBy { get; }
    Expression<Func<TEntity, object>> OrderByDescending { get; }


    List<string> IncludeStrings { get; }
    Expression<Func<TEntity, object>> GroupBy { get; }

    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
