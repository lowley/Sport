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
    public Exercise _currentExercise;

    [ObservableProperty]
    public ExerciceDifficulty _currentDifficulty;

    [RelayCommand]
    public async Task Save()
    {
        if (CurrentDifficulty.DifficultyLevel == 0 || string.IsNullOrEmpty(CurrentExercise.ExerciseName))
            return;

        if (ExercisesVM._exercices.Any(oneExercise => 
            oneExercise.Id.Equals(CurrentExercise.Id)
            && oneExercise.ExerciseName.Equals(CurrentExercise.ExerciseName)
            && oneExercise.ExerciseDifficulties.Any(oneDifficulty =>
                oneDifficulty == CurrentDifficulty)))
            return;
        
        var existingExercise = ExercisesVM._exercices.FirstOrDefault(oneExercice =>
            oneExercice.Id == CurrentExercise.Id);
        
        if (existingExercise is not null)
        {
            //l'exercice existe déjà
            if (!CurrentExercise.ExerciseName.Equals(existingExercise.ExerciseName))
                existingExercise.ExerciseName = CurrentExercise.ExerciseName;
            
            if (existingExercise.ExerciseDifficulties.All(diff => diff.Id != CurrentDifficulty.Id))
                existingExercise.ExerciseDifficulties.Add(CurrentDifficulty);
        }
        else
        {
            //nouvel exercice
            
            //on veut en créer un avec un nom existant
            if (ExercisesVM._exercices.Any(oneExercise => CurrentExercise.SameAs(oneExercise)))
                return;
            
            CurrentExercise.ExerciseDifficulties.Add(CurrentDifficulty);
            ExercisesVM._exercices.Add(CurrentExercise);
        }

        CurrentDifficulty = new (0, "Kg");
    }

    [RelayCommand]
    public async Task Back()
    {
        await Shell.Current.Navigation.PopAsync(false);
    }

    public ExerciseVM()
    {
        CurrentDifficulty = new(0, "Kg");
        CurrentExercise = new();
    }
}
