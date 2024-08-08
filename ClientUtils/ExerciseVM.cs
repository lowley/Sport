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
        //Trace.WriteLine($" en:{ExerciseName}, dn:{Difficulty.DifficultyName}, dl:{Difficulty.DifficultyLevel}");


        if (CurrentDifficulty.DifficultyLevel == 0)
            return;

        if (ExercisesVM._exercices.Any(
                x => x.ExerciseName == CurrentExerciseName
                && x.ExerciseDifficulties.Any(ed => ed.DifficultyName == CurrentDifficulty.DifficultyName)
                && x.ExerciseDifficulties.Any(ed => ed.DifficultyLevel == CurrentDifficulty.DifficultyLevel)))
            return;

        if (ExercisesVM._exercices.Any(x => x.ExerciseName == CurrentExerciseName))
        {
            var exercise = ExercisesVM._exercices.FirstOrDefault(x => x.ExerciseName == CurrentExerciseName);
            exercise?.ExerciseDifficulties.Add(CurrentDifficulty);
        }
        else
        {
            var exercise = new ExerciseEntity();
            exercise.ExerciseName = CurrentExerciseName;
            exercise.ExerciseDifficulties.Add(CurrentDifficulty);
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
