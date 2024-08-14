using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
