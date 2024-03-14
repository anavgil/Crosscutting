﻿using Crosscutting.Persistence.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crosscutting.Persistence.Repositories.Implementation;

public class Repository<TEntity, T, TContext> : IRepository<TEntity, T>
    where TEntity : class, new()
    where T : IComparable, IEquatable<T>
    where TContext : DbContext
{
    public const int PAGINATION_SIZE = 20;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DbSet = context.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Add(entity);
    }

    public void Add(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        DbSet.AddRange(entities);
    }

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        DbSet.RemoveRange(entities);
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        List<Expression<Func<TEntity, object>>> includes = null,
                                                        bool disableTracking = true, CancellationToken ct = default)
    {
        IQueryable<TEntity> query = DbSet;

        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter != null) query = query.Where(filter);

        if (orderBy != null)
            return await orderBy(query).ToListAsync(ct);

        return await query.ToListAsync(ct);
    }

    public async Task<IPaginate<TEntity>> GetPaginatedAsync(Expression<Func<TEntity, bool>> filter = null,
                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                List<Expression<Func<TEntity, object>>> includes = null,
                                int index = 0, int size = PAGINATION_SIZE,
                                bool disableTracking = true,
                                CancellationToken ct = default)
    {
        IQueryable<TEntity> query = DbSet;

        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter != null) query = query.Where(filter);

        if (orderBy != null)
            return await orderBy(query).ToPagedListAsync<TEntity>(index, PAGINATION_SIZE, ct);

        return await query.ToPagedListAsync<TEntity>(index, PAGINATION_SIZE, ct);
    }

    public TEntity GetSingle(Expression<Func<TEntity, bool>> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        return DbSet.SingleOrDefault(condition);
    }

    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition,CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(condition);

        return await DbSet.SingleOrDefaultAsync(condition,ct);
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        DbSet.UpdateRange(entities);
    }
}
