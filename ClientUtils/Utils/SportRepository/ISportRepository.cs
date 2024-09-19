using System.Linq.Expressions;
using ClientUtilsProject.DataClasses;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClientUtilsProject.Utils.SportRepository;

public interface ISportRepository
{
    Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class;
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class;
    Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;
    Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    Task LikeUpdateAsync<TEntity>(TEntity entity) where TEntity : class;
    void Attach<TEntity>(TEntity entity) where TEntity : class;
    Task RemoveAsync<TEntity>(TEntity entity) where TEntity : class;
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ReloadAsync();
    SportContext? GetContext();
    Task DisposeAsync();
    Task Clear();
}