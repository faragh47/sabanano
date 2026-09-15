using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Common.Extensions;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
//using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
//using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContext :IdentityDbContext<ApplicationUser, AccRole, long>, IApplicationDbContext// ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;


    public ApplicationDbContext(DbContextOptions options, IMediator mediator, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
          : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    //public ApplicationDbContext(
    //    DbContextOptions<ApplicationDbContext> options,
    //    IOptions<OperationalStoreOptions> operationalStoreOptions,
    //    IMediator mediator,
    //    AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
    //    : base(options, operationalStoreOptions)
    //{
    //    _mediator = mediator;
    //    _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    //}

    DbSet<TEntity> IApplicationDbContext.Set<TEntity>()
    {
        DbSet<TEntity> result = Set<TEntity>();
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        var entitiesAssembly = typeof(IEntity).Assembly;
        base.OnModelCreating(builder);
        builder.RegisterAllEntities<IEntity>(entitiesAssembly);
        builder.RenameIdentityTableName();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents<int>(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    int IApplicationDbContext.SaveChanges()
    {
        return base.SaveChanges();
    }

    public async Task<int> DeActiveSaveChangesAsync(CancellationToken cancellationToken)
    {
        await _mediator.DispatchDomainEvents<int>(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
