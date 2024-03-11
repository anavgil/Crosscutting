using Crosscutting.Persistence.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crosscutting.Persistence.Repositories.Implementation;

public class Repository<TEntity, T, TContext> : IRepository<TEntity, T>
    where TEntity : class, new()
    where T : IComparable, IEquatable<T>
    where TContext : DbContext
{
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

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
    }

    public IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> condition = null)
    {
        return condition != null ? DbSet.Where(condition).AsEnumerable() : DbSet.AsEnumerable();
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
