using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class SessionExerciceSerie : SportEntity
{
    public Guid Id;

    [ObservableProperty]
    public ExerciceDifficulty _difficulty;

    [ObservableProperty]
    public Int32 _repetitions;

    [ObservableProperty]
    public Int32 _nombreDeFoisDeLaSerie;

}