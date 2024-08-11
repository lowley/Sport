using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class SessionExerciceSerie : ObservableObject
{
    [ObservableProperty]
    public ExerciceDifficulty _difficulty;

    [ObservableProperty]
    public Int32 _repetitions;

    [ObservableProperty]
    public Int32 _nombreDeLaSerie;

}