using System.Linq.Expressions;

namespace Crosscutting.Persistence.Abstractions.Repositories;

public class ISpecification<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, bool>> Criteria { get; }
    public List<Expression<Func<TEntity, object>>> Includes { get; }
    public Expression<Func<TEntity, object>> OrderBy { get; }
    public Expression<Func<TEntity, object>> OrderByDescending { get; }
}
