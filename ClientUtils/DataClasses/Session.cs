using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Query;
using Groups =
    System.Collections.ObjectModel.ObservableCollection<ClientUtilsProject.DataClasses.Group>;

namespace ClientUtilsProject.DataClasses;

public partial class Session : SportEntity
{
    [ObservableProperty] public DateTime _sessionStartDate = DateTime.Now;

    [ObservableProperty] public TimeSpan _sessionStartTime = DateTime.Now.TimeOfDay;

    [ObservableProperty] public TimeSpan _sessionEndTime = DateTime.Now.TimeOfDay;

    [ObservableProperty] public ObservableCollection<SessionExerciceSerie> _sessionItems = [];

    [ObservableProperty] public Boolean _isOpened;

    [NotMapped] [ObservableProperty] public Groups _groupedSessionItems = [];

    public Session()
    {
        Id = Guid.NewGuid();
        _sessionItems = [];

        //SessionItems.CollectionChanged += (sender, args) => { ModifySessionItems(); };
    }

    public void ModifySessionItems()
    {
        Groups result = [];

        //1er passage: regroupement par ExerciseName
        while (result.Sum(g => g.Series.Count) < SessionItems.Count)
        {
            var newExerciseName = SessionItems
                .FirstOrDefault(ses => !result.Select(g => g.Name).Contains(ses.Exercice.ExerciseName))
                ?.Exercice.ExerciseName;

            //bug théorique
            if (newExerciseName == null)
                return;
            
            var currentGroup = new Group();
            currentGroup.Name = newExerciseName;

            foreach (var serie in SessionItems)
            {
                if (serie.Exercice.ExerciseName.Equals(newExerciseName))
                    currentGroup.Series.Add(serie);
            }
            
            result.Add(currentGroup);
        }
        
        //2e passage dans les groupes: regroupement des (DifficultyLevel*DifficultyName, Repetitions)
        Groups newResult = [];
        foreach (var group in result)
        {
            var currentNewGroup = new Group();
            currentNewGroup.Name = group.Name;
            
            foreach (var serie in group.Series)
            {
                //bug théorique
                if (serie == null)
                    return;

                var sameSerie = currentNewGroup.Series
                    .FirstOrDefault(ses => ses.Difficulty.DifficultyLevel == serie.Difficulty.DifficultyLevel &&
                                           ses.Difficulty.DifficultyName.Equals(serie.Difficulty.DifficultyName) &&
                                           ses.Repetitions == serie.Repetitions);

                if (sameSerie != null)
                    sameSerie.Series += 1;
                else
                    currentNewGroup.Series.Add(serie);
            }
            
            newResult.Add(currentNewGroup);
        }
        
        GroupedSessionItems.Clear();
        foreach (var grp in newResult)
            GroupedSessionItems.Add(grp);
    }

    public void RaisePropertyChanged(string prop)
    {
        OnPropertyChanged(prop);
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