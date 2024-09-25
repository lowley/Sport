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
    [NotifyPropertyChangedFor(nameof(ShowSummary))]
    public ExerciceDifficulty _difficulty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowSummary))]
    public Int32 _series;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowSummary))]
    public Int32 _repetitions;
    
    public string ShowSummary => $"{Difficulty.ShowMeShort}*{Repetitions}:{Series}";
    
    public SessionExerciceSerie()
    {
        Id = Guid.NewGuid();
        Difficulty = new ExerciceDifficulty();
        DifficultyId = Guid.NewGuid();
        Exercice = new Exercise();
        ExerciceId = Guid.NewGuid();
    }
}