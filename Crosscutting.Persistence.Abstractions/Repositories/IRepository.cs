﻿using System.Linq.Expressions;

namespace Crosscutting.Persistence.Abstractions.Repositories;



public interface IRepository<TEntity, T>
    where TEntity : class, new()
    where T : IComparable, IEquatable<T>
{
    Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                List<Expression<Func<TEntity, object>>>? includes = null,
                                bool disableTracking = true,
                                CancellationToken ct = default);

    Task<IPaginate<TEntity>> GetPaginatedAsync(Expression<Func<TEntity, bool>>? filter = null,
                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                    List<Expression<Func<TEntity, object>>>? includes = null,
                                    int index = 0, int size = 0,
                                    bool disableTracking = true,
                                    CancellationToken ct = default);

    TEntity GetSingle(Expression<Func<TEntity, bool>> condition);
    void Add(TEntity entity);
    void Add(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Update(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void Delete(IEnumerable<TEntity> entities);
}
