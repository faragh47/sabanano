using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class, IEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> DeActiveSaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}
