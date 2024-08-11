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
    public ExerciceDifficulty _currentDifficulty;

    [RelayCommand]
    public async Task Save()
    {
        if (CurrentDifficulty.DifficultyLevel == 0)
            return;

        var currentExercise = new Exercise();
        currentExercise.ExerciseName = CurrentExerciseName;
        currentExercise.ExerciseDifficulties.Add(CurrentDifficulty);

        if (ExercisesVM._exercices.Any(oneExercise => 
            oneExercise.ExerciseName.Equals(currentExercise.ExerciseName)
            && oneExercise.ExerciseDifficulties.Any(oneDifficulty =>
                oneDifficulty == CurrentDifficulty)))
            return;

        if (ExercisesVM._exercices.Any(x => x.ExerciseName == CurrentExerciseName))
        {
            currentExercise = ExercisesVM._exercices.FirstOrDefault(x => x.ExerciseName == CurrentExerciseName);
            currentExercise?.ExerciseDifficulties.Add(CurrentDifficulty);
        }
        else
        {
            ExercisesVM._exercices.Add(currentExercise);
        }

        CurrentDifficulty = new ();
    }

    [RelayCommand]
    public async Task Back()
    {
        await Shell.Current.Navigation.PopAsync(false);
    }

    public ExerciseVM()
    {
        CurrentDifficulty = new();

    }
}
