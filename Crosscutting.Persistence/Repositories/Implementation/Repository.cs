﻿using Crosscutting.Persistence.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crosscutting.Persistence.Repositories.Implementation;

public class Repository<TEntity, T, TContext> : IRepository<TEntity, T>
    where TEntity : class, IEntity<T>, new()
    where T : IComparable, IEquatable<T>
    where TContext : DbContext
{
    private readonly TContext _context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DbSet = context.Set<TEntity>();
        _context = context;
    }

    public void Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Add(entity);
    }

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
    }

    public IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> condition = null)
    {
        return condition != null ? DbSet.Where(condition).AsEnumerable() : DbSet.AsEnumerable();
    }

    public TEntity GetSingle(T id)
    {
        if (id.Equals(default)) throw new ArgumentNullException(nameof(id));

        return DbSet.SingleOrDefault(x => x.Id.Equals(id));
    }

    public TEntity GetSingle(Expression<Func<TEntity, bool>> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        return DbSet.SingleOrDefault(condition);
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Update(entity);
    }
}
