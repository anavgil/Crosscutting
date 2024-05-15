using System.Linq.Expressions;

namespace Crosscutting.Persistence.Abstractions.Specifications;

public class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
{
    public new Expression<Func<TEntity, bool>> Criteria { get; }
    public new List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    public new Expression<Func<TEntity, object>> OrderBy { get; private set; }
    public new Expression<Func<TEntity, object>> OrderByDescending { get; private set; }
    public new Expression<Func<TEntity, object>> GroupBy { get; private set; }

    public new int Take { get; private set; }
    public new int Skip { get; private set; }
    public new bool IsPagingEnabled { get; private set; } = false;

    public Specification()
    {
    }

    public Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
