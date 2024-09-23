using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class SessionExerciceSerie : SportEntity
{
    public Guid SessionId { get; set; }
    [ObservableProperty]
    public Session _session;
    
    public Guid ExerciceId { get; set; }
    [ObservableProperty]
    public Exercise _exercice;
    
    public Guid DifficultyId { get; set; }
    [ObservableProperty]
    public ExerciceDifficulty _difficulty;

    [ObservableProperty]
    public Int32 _series;
    
    [ObservableProperty]
    public Int32 _repetitions;
    
    public SessionExerciceSerie()
    {
        Id = Guid.NewGuid();
        Difficulty = new ExerciceDifficulty();
        DifficultyId = Guid.NewGuid();
        Exercice = new Exercise();
        ExerciceId = Guid.NewGuid();
    }
}