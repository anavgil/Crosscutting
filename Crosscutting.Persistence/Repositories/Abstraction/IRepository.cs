using System.Linq.Expressions;

namespace Crosscutting.Persistence.Repositories.Abstraction;

public interface IRepository<TEntity, T>
    where TEntity : class, IEntity<T>, new()
    where T : IComparable, IEquatable<T>
{
    TEntity GetSingle(T id);
    TEntity GetSingle(Expression<Func<TEntity, bool>> condition);
    IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> condition = null);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
