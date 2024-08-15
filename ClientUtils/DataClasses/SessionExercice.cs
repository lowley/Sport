using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class SessionExercice : SportEntity
{
    public Guid Id { get; set; }

    public Guid ExerciceId { get; set; }

    [ObservableProperty]
    public Exercise _exercice;

    [ObservableProperty]
    public ObservableCollection<SessionExerciceSerie> _series;

    public SessionExercice()
    {
        Id = Guid.NewGuid();
        Exercice = new Exercise();
        Series = [];
        ExerciceId = Guid.NewGuid();
    }
}