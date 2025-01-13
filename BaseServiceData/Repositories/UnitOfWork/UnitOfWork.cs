using System.Collections;
using BaseServiceContracts.Interfaces.Repositories.Base;
using BaseServiceContracts.Interfaces.Repositories.Implementations;
using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceData.Contexts.PsSql;
using BaseServiceData.Repositories.Base;
using BaseServiceData.Repositories.Implementations;
using BaseServiceLibrary.Entity.Base;
using BaseServiceLibrary.Enum.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BaseServiceData.Repositories.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly PsSqlApplicationDataContext _dbContext;    
    private readonly Hashtable _repositories;
    private bool _disposed;

    public UnitOfWork(IDbContextFactory<PsSqlApplicationDataContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _repositories = new Hashtable();
    }

    public async Task OutboxRegistrationAsync<T>(T entity, OutboxOperationType operationType = OutboxOperationType.CreateUpdate, CancellationToken cancellationToken = default)
    {
        OutboxMessage item = OutboxMessage.Create(entity, operationType);
        await _dbContext.OutboxMessages
            .AddAsync(item, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database
            .CommitTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database
            .RollbackTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public void ClearCache()
    {
        EntityEntry[] changedEntriesCopy = _dbContext.ChangeTracker.Entries().ToArray();

        foreach (EntityEntry entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }

    public async Task OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.OpenConnectionAsync(cancellationToken);
    }

    public async Task CloseConnectionAsync()
    {
        await _dbContext.Database.CloseConnectionAsync();
    }

    public T GetRepository<T>() where T : class
    {
        string type = typeof(T).Name;
        if (_repositories.ContainsKey(type))
            return (T)_repositories[type];

        object repo = new object();
        
        if (typeof(T) == typeof(IPartnerZoneRepository))
        {
            repo = new PartnerZoneRepository(_dbContext);
        }
        
        _repositories.Add(type, repo);

        return (T)repo;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        int result = await _dbContext.SaveChangesAsync(cancellationToken);
        _dbContext.ChangeTracker.Clear();
        return result;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        _disposed = true;
    }
}