using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace ClientUtilsProject.ViewModels;

public partial class ExerciseVM : ObservableObject
{
    [ObservableProperty] 
    public Exercise _currentExercise;

    [ObservableProperty]
    public ExerciceDifficulty _currentDifficulty;

    private ISportRepository Repository { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }

    
    [RelayCommand]
    public async Task Save()
    {
        if (CurrentDifficulty.DifficultyLevel == 0 || string.IsNullOrEmpty(CurrentExercise.ExerciseName))
            return;

        var exerciseInDatabase = Repository.Query<Exercise>()
            .Where(e => e.Id == CurrentExercise.Id)
            .Include(e => e.ExerciseDifficulties)
            .FirstOrDefault();
        
        if (exerciseInDatabase is not null &&
            exerciseInDatabase.ExerciseName.Equals(CurrentExercise.ExerciseName) &&
            exerciseInDatabase.ExerciseDifficulties.Any(oneDifficulty =>
                oneDifficulty == CurrentDifficulty))
            return;
        
        if (exerciseInDatabase is not null)
        {
            //l'exercice existe déjà
            // changement de nom?
            if (!CurrentExercise.ExerciseName.Equals(exerciseInDatabase.ExerciseName))
                exerciseInDatabase.ExerciseName = CurrentExercise.ExerciseName;
            
            // ajout de difficulté?
            if (exerciseInDatabase.ExerciseDifficulties.All(diff => diff.Id != CurrentDifficulty.Id))
                exerciseInDatabase.ExerciseDifficulties.Add(CurrentDifficulty);
            
            //await Repository.UpdateAsync(exerciseInDatabase);
            await Repository.SaveChangesAsync(CancellationToken.None);
        }
        else
        {
            //nouvel exercice
            
            //on veut en créer un avec un nom existant
            if (ExercisesVM._exercices.Any(oneExercise => CurrentExercise.SameAs(oneExercise)))
                return;
            
            CurrentExercise.ExerciseDifficulties.Add(CurrentDifficulty);

            await Repository.AddAsync(CurrentExercise);
            await Repository.SaveChangesAsync(CancellationToken.None);
            
        }

        CurrentDifficulty = new (0, "Kg");
    }

    [RelayCommand]
    public async Task Back()
    {
        await Navigation.NavigateBack();
    }

    public ExerciseVM(
        ISportNavigation navigation, 
        ISportRepository repository,
        ISportLogger logger)
    {
        Navigation = navigation;
        CurrentDifficulty = new(0, "Kg");
        CurrentExercise = new();
        Repository = repository;
        Logger = logger;
    }
}
