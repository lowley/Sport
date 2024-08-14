using System.CodeDom;
using System.Linq.Expressions;
using ClientUtilsProject.DataClasses;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClientUtilsProject.Utils.SportRepository;

public class SportRepository : ISportRepository
{
    private SportContext Context { get; set; }


    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        Object o = entity switch
        {
            Exercise e => await Context.AddAsync(entity as Exercise),
            ExerciceDifficulty ed => await Context.AddAsync(entity as ExerciceDifficulty),
            Session s => await Context.AddAsync(entity as Session),
            SessionExercice se => await Context.AddAsync(entity as SessionExercice),
            SessionExerciceSerie ses => await Context.AddAsync(entity as SessionExerciceSerie),

            _ => throw new ArgumentException()
        };

        return;
    }

    public void Clear()
    {
        Context.Clear();
    }
    
    /*public IQueryable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> selector)
    {
        throw new NotImplementedException();
    }
    */
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
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