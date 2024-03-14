using Crosscutting.Persistence.Abstractions.Repositories;
using Crosscutting.Persistence.Abstractions.UoW;
using Crosscutting.Persistence.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Crosscutting.Persistence.UoW.Implementation;

public class UnitOfWork<TContext>(TContext context) : IUnitOfWork, IDisposable
    where TContext : DbContext
{
    private bool disposed = false;
    private readonly Dictionary<Type, object> repositories = [];

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
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

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, new()
    {
        if (repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity, TContext>(context);
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
