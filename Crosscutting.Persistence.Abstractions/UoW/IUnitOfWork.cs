using Crosscutting.Persistence.Abstractions.Repositories;

namespace Crosscutting.Persistence.Abstractions.UoW;
public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync(CancellationToken cancellationToken = default);
    void Rollback();

    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, new();
}
