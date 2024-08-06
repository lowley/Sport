using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageExt;
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
    public ExerciseEntity _exercise = new();
   
    [RelayCommand]
    public async Task Save()
    {
        //Trace.WriteLine($" en:{ExerciseName}, dn:{Difficulty.DifficultyName}, dl:{Difficulty.DifficultyLevel}");

        if (string.IsNullOrEmpty(Exercise.ExerciseDifficulty.DifficultyName) || Exercise.ExerciseDifficulty.DifficultyLevel == 0)
            return;

        if (ExercisesVM._exercices.Any(
                x => x.ExerciseName == Exercise.ExerciseName
                && x.ExerciseDifficulty.DifficultyName == Exercise.ExerciseDifficulty.DifficultyName
                && x.ExerciseDifficulty.DifficultyLevel == Exercise.ExerciseDifficulty.DifficultyLevel))
            return;

        ExercisesVM._exercices.Add(Exercise);

        await Shell.Current.Navigation.PopAsync(false);
    }


    public ExerciseVM()
    {
        
    }
}
