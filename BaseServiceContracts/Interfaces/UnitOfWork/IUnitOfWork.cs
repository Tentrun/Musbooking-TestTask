using BaseServiceContracts.Interfaces.Repositories.Base;
using BaseServiceLibrary.Entity.Base;
using BaseServiceLibrary.Enum.Base;

namespace BaseServiceContracts.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    T GetRepository<T>() where T : class;

    Task<int> SaveAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    void ClearCache();
    Task OpenConnectionAsync(CancellationToken cancellationToken = default);
    Task CloseConnectionAsync();
    Task OutboxRegistrationAsync<T>(T entity, OutboxOperationType operationType = OutboxOperationType.CreateUpdate, CancellationToken cancellationToken = default);
}