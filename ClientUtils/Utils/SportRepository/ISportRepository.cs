using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClientUtilsProject.Utils.SportRepository;

public interface ISportRepository
{
    public Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
    public IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    public Task DisposeAsync();
    public void Clear();


}