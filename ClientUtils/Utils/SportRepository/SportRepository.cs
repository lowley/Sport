using System.CodeDom;
using System.Linq.Expressions;
using ClientUtilsProject.DataClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClientUtilsProject.Utils.SportRepository;

public class SportRepository : ISportRepository
{
    private SportContext Context { get; set; }


    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        
        await Context.Set<TEntity>().AddAsync(entity);
        
        // Object o = entity switch
        // {
        //     Exercise e => await Context.AddAsync(entity as Exercise),
        //     ExerciceDifficulty ed => await Context.AddAsync(entity as ExerciceDifficulty),
        //     Session s => await Context.AddAsync(entity as Session),
        //     SessionExercice se => await Context.AddAsync(entity as SessionExercice),
        //     SessionExerciceSerie ses => await Context.AddAsync(entity as SessionExerciceSerie),
        //
        //     _ => throw new ArgumentException()
        // };
    }

    public async Task Clear()
    {
        await Context.ExerciceDifficulties.ExecuteDeleteAsync();
        await Context.Exercises.ExecuteDeleteAsync();
        await Context.Sessions.ExecuteDeleteAsync();
        await Context.SessionExercices.ExecuteDeleteAsync();
        await Context.SessionExerciceSeries.ExecuteDeleteAsync();
    }

    public void Attach<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Set<TEntity>().Attach(entity);
    }

    public async Task RemoveAsync<TEntity>(TEntity entity) where TEntity: class
    {
        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReloadAsync()
    {
        await Context.ReloadAsync();
    }

    public SportContext GetContext()
    {
        return Context;
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity: class
    {
        return Context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity: class
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        return await Context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task LikeUpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        // await Context.Set<TEntity>().ExecuteUpdateAsync(entity);
        // await Context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Context.DisposeAsync();
    }

    public SportRepository(SportContext context)
    {
        Context = context;
    }
}