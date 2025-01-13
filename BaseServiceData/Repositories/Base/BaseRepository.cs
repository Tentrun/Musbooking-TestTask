using System.Linq.Expressions;
using BaseServiceContracts.Interfaces.Repositories.Base;
using BaseServiceData.Contexts.PsSql;
using BaseServiceLibrary.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace BaseServiceData.Repositories.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly PsSqlApplicationDataContext DbContext;

    public BaseRepository(PsSqlApplicationDataContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public IQueryable<T> Entities => DbContext.Set<T>();
    
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int maxResultSize = 0, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<T>()
            .AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (maxResultSize > 0)
        {
            query = query.Take(maxResultSize);
        }

        return await query
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Entry(entity).State = EntityState.Added;
        
        return Task.FromResult(entity);
    }

    public virtual async Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<T>()
            .AddRangeAsync(entities, cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Entry(entity).State = EntityState.Modified;

        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            UpdateAsync(entity, cancellationToken);
        }
        return Task.CompletedTask;
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Remove(entity);
        
        return Task.CompletedTask;
    }

    public Task SaveChangeHistoryAsync(ChangeHistory entity, CancellationToken cancellationToken = default)
    {
        DbContext.Entry(entity).State = EntityState.Added;
        
        return Task.CompletedTask;
    }
}