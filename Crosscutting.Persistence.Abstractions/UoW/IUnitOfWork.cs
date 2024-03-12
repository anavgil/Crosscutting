﻿using Crosscutting.Persistence.Abstractions.Repositories;

namespace Crosscutting.Persistence.Abstractions.UoW;
public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync(CancellationToken cancellationToken = default);
    void Rollback();

    IRepository<TEntity, T> GetRepository<TEntity, T>()
        where TEntity : class, new()
        where T : IComparable, IEquatable<T>;
}
