using System.Linq.Expressions;

namespace Crosscutting.Persistence.Abstractions.Specifications;

public class ISpecification<TEntity>
{
    public Expression<Func<TEntity, bool>> Criteria { get; }
    public List<Expression<Func<TEntity, object>>> Includes { get; }
    public Expression<Func<TEntity, object>> OrderBy { get; }
    public Expression<Func<TEntity, object>> OrderByDescending { get; }


    public List<string> IncludeStrings { get; }
    public Expression<Func<TEntity, object>> GroupBy { get; }

    public int Take { get; }
    public int Skip { get; }
    public bool IsPagingEnabled { get; }
}
