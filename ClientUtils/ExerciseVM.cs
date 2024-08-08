using ClientUtilsProject.DataClasses;
using ClientUtilsProject.DataClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageExt;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.VM;

public partial class ExerciseVM : ObservableObject
{
    [ObservableProperty]
    public string _currentExerciseName;

    [ObservableProperty]
    public DifficultyContainer _currentDifficulty = new(11, "Kg");

    [RelayCommand]
    public async Task Save()
    {
        if (CurrentDifficulty.DifficultyLevel == 0)
            return;

        var exercise = new ExerciseEntity();
        exercise.ExerciseName = CurrentExerciseName;
        exercise.ExerciseDifficulties.Add(CurrentDifficulty);

        if (ExercisesVM._exercices.Any(x => x == exercise))
            return;

        if (ExercisesVM._exercices.Any(x => x.ExerciseName == CurrentExerciseName))
        {
            exercise = ExercisesVM._exercices.FirstOrDefault(x => x.ExerciseName == CurrentExerciseName);
            exercise?.ExerciseDifficulties.Add(CurrentDifficulty);
        }
        else
        {
            ExercisesVM._exercices.Add(exercise);
            CurrentDifficulty = new DifficultyContainer(0, "Kg");
        }
    }

    [RelayCommand]
    public async Task Back()
    {
        await Shell.Current.Navigation.PopAsync(false);
    }

    public ExerciseVM()
    {

    }
}
