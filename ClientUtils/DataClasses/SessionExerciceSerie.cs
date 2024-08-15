using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class SessionExerciceSerie : SportEntity
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    public Int32 _repetitions;

    [ObservableProperty]
    public Int32 _nombreDeFoisDeLaSerie;

    public Guid DifficultyId { get; set; }

    [ObservableProperty]
    public ExerciceDifficulty _difficulty;
    
    public SessionExerciceSerie()
    {
        Id = Guid.NewGuid();
        Difficulty = new ExerciceDifficulty();
        DifficultyId = Guid.NewGuid();

    }
}