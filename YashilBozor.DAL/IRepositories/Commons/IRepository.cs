﻿using System.Linq.Expressions;
using YashilBozor.Domain.Entities.Commons;

namespace YashilBozor.DAL.IRepositories.Commons;

public interface IRepository<TEntity> where TEntity : Auditable
{
    ValueTask<TEntity> DeleteAsync(
        Guid id,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    );

    IQueryable<TEntity> SelectAll(
        Expression<Func<TEntity, bool>>? predicate = default,
        bool asNoTracking = false);

    ValueTask<TEntity> SelectByIdAsync(
        Guid id,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    ValueTask<TEntity> InsertAsync(
        TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    );

    ValueTask<TEntity> UpdateAsync(
        TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    );
}
