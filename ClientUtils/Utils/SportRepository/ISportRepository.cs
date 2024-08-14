using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClientUtilsProject.Utils.SportRepository;

public interface ISportRepository
{
    public Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
    //IQueryable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> selector);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task DisposeAsync();
    public void Clear();


}