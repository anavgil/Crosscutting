using Crosscutting.Persistence.Abstractions.UoW;
using Crosscutting.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Crosscutting.Persistence.UoW;

public class UnitOfWork(DbContext context, Dictionary<Type, object> repositories) : IUnitOfWork
{
    private bool disposed = false;



    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }

    
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(context);
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
