using ClientUtilsProject.Utils;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;

namespace ClientUtilsProject.DataClasses;

public class SportContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciceDifficulty> ExerciceDifficulties { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionExercice> SessionExercices { get; set; }
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

    public void Reload()
    {
        ChangeTracker.Entries().ToList().ForEach(e => e.Reload());
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
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExerciceDifficulty>()
            .Property(e => e.DifficultyName)
            .HasConversion(new OptionStringConverter());
    }

    public void Clear()
    {
        Database.EnsureDeleted();
    }
}