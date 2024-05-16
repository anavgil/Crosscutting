﻿using System.Linq.Expressions;

namespace Crosscutting.Persistence.Abstractions.Specifications;

public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
{
    public new Expression<Func<TEntity, bool>> Criteria { get; private set; }
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

    protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected virtual void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected virtual void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    protected virtual void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
