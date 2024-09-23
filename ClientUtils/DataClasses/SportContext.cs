using ClientUtilsProject.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog.Core;

namespace ClientUtilsProject.DataClasses;

public class SportContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciceDifficulty> ExerciceDifficulties { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionExerciceSerie> SessionExerciceSeries { get; set; }

    public DateTime InitTime { get; set; }
    public ISportLogger Logger { get; set; }
    
    public SportContext(ISportLogger logger) : base()
    {
        Logger = logger;
        SQLitePCL.Batteries.Init();
        var b = Database.EnsureCreated();
        InitTime = DateTime.Now;
    }

    public async Task ReloadAsync()
    {
        ChangeTracker.Entries().ToList().ForEach(async e => await e.ReloadAsync());
    }

    public void Reload<TEntity>(TEntity entity) where TEntity : class
    {
        Entry(entity).Reload();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "sport.db3");

        optionsBuilder.EnableSensitiveDataLogging();

        optionsBuilder
            .UseSqlite($"Filename={dbPath}");

        optionsBuilder.LogTo(s => Logger.Information(s));
        optionsBuilder.AddInterceptors([new MyCommandInterceptor(Logger)]);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExerciceDifficulty>()
            .Property(e => e.DifficultyName)
            .HasConversion(new OptionStringConverter());
        
        //le code suivant vient de StackOverflow
        //sinon pb d'update d'Exercise quand on ajoute une ExerciseDifficulty
        //le pb est lié aux clés primaires générées manuellement et non par EF
        foreach (var item in modelBuilder.Model.GetEntityTypes())
        {
            // if (item.ClrType == typeof(Grouping))
            //     continue;
            
            var p = item.FindPrimaryKey().Properties.FirstOrDefault(i=>i.ValueGenerated!=Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never);
            if (p!=null)
            {
                p.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never;
            }
        }
    }

    public void Clear()
    {
        Database.EnsureDeleted();
    }
}