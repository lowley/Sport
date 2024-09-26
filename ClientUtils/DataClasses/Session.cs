using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Groups =
    System.Collections.ObjectModel.ObservableCollection<ClientUtilsProject.DataClasses.Group>;

namespace ClientUtilsProject.DataClasses;

public partial class Session : SportEntity
{
    [ObservableProperty] public DateTime _sessionStartDate = DateTime.Now;

    [ObservableProperty] public TimeSpan _sessionStartTime = DateTime.Now.TimeOfDay;

    [ObservableProperty] public TimeSpan _sessionEndTime = DateTime.Now.TimeOfDay;

    [ObservableProperty] public ObservableCollection<SessionExerciceSerie> _sessionItems = [];

    [NotMapped] [ObservableProperty] public Groups _groupedSessionItems = [];

    public Session()
    {
        Id = Guid.NewGuid();
        _sessionItems = [];

        SessionItems.CollectionChanged += (sender, args) =>
        {
            Groups result = [];

            if (SessionItems.Any())
                result.Add(new Group());

            foreach (var serie in SessionItems)
            {
                if (result.Last().Name.Equals(string.Empty))
                {
                    result.Last().Name = serie.Exercice.ExerciseName;
                    result.Last().Series = new() { serie };

                    continue;
                }

                if (result.Last().Series.Last().ExerciceId.Equals(serie.Exercice.Id))
                {
                    if (result.Last().Series.Last().Difficulty.DifficultyName
                            .Equals(serie.Difficulty.DifficultyName) &&
                        result.Last().Series.Last().Difficulty.DifficultyLevel == serie.Difficulty.DifficultyLevel &&
                        result.Last().Series.Last().Repetitions == serie.Repetitions
                       )

                        result.Last().Series.Last().Series += 1;
                    else
                        result.Last().Series.Add(serie);

                    continue;
                }

                result.Add(new Group()
                {
                    Name = serie.Exercice.ExerciseName,
                    Series = new() { serie }
                });
            }

            GroupedSessionItems.Clear();
            foreach (var grp in result)
                GroupedSessionItems.Add(grp);
        };
    }
}

public class Group
{
    public string Name { get; set; }
    public ObservableCollection<SessionExerciceSerie> Series { get; set; }

    public Group(string name = "", ObservableCollection<SessionExerciceSerie> series = null)
    {
        Name = name;
        Series = series ?? [];
    }
}