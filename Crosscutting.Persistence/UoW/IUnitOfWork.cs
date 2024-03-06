using Crosscutting.Persistence.Repositories;

namespace Crosscutting.Persistence.Abstractions.UoW;
public interface IUnitOfWork:IDisposable
{
    Task CompleteAsync();
    void Rollback();
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}
