﻿using Crosscutting.Persistence.Abstractions.Repositories;
using System.Linq.Expressions;

namespace Crosscutting.Persistence.Repositories.Implementation;

public class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
{
    public new Expression<Func<TEntity, bool>> Criteria { get; }
    public new List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    public new Expression<Func<TEntity, object>> OrderBy { get; private set; }
    public new Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

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

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}
