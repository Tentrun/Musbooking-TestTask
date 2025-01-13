using System.Linq.Expressions;
using BaseServiceLibrary.Entity.Base;

namespace BaseServiceContracts.Interfaces.Repositories.Base;

public interface IBaseRepository<T> where T : class
{
    IQueryable<T> Entities { get; }
        
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int maxResultSize = 0, CancellationToken cancellationToken = default);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        
    Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    
    Task SaveChangeHistoryAsync(ChangeHistory entity, CancellationToken cancellationToken = default);
}