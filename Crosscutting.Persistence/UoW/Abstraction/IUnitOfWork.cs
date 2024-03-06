using Crosscutting.Persistence.Repositories.Abstraction;

namespace Crosscutting.Persistence.UoW.Abstraction;
public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync();
    void Rollback();
    IRepository<TEntity,T> GetRepository<TEntity,T>() 
        where TEntity : class,IEntity<T>,new()
        where T : IComparable, IEquatable<T>;
}
