using Crosscutting.Persistence.Repositories.Abstraction;
using Crosscutting.Persistence.Repositories.Implementation;
using Crosscutting.Persistence.UoW.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Crosscutting.Persistence.UoW.Implementation;

public class UnitOfWork<TContext>(TContext context) : IUnitOfWork, IDisposable
    where TContext :DbContext
{
    private bool disposed = false;
    private readonly Dictionary<Type, object> repositories = [];

    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IRepository<TEntity, T> GetRepository<TEntity, T>()
                        where TEntity : class, IEntity<T>, new()
                        where T : IComparable, IEquatable<T>
    {
        if (repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity, T>)repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity, T, TContext>(context);
        repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public void Rollback()
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }
}
