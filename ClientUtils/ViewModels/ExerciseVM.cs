using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace ClientUtilsProject.ViewModels;

public partial class ExerciseVM : ObservableObject
{
    [ObservableProperty] public ObservableCollection<Exercise> _exercises;

    [ObservableProperty] public Exercise _currentExercise;

    [ObservableProperty] public ExerciceDifficulty _currentDifficulty;

    [ObservableProperty] public Exercise _selectedExercise;

    [ObservableProperty] public string _newExerciseName;

    [ObservableProperty] public string _existingExerciseName;

    [ObservableProperty] public bool _existingExerciseNameInError;

    private ISportRepository Repository { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }


    partial void OnSelectedExerciseChanged(Exercise value)
    {
        if (value is null)
        {
            CurrentExercise = new();
            return;
        }

        CurrentExercise = value;
        NewExerciseName = string.Empty;
        CurrentDifficulty = new ExerciceDifficulty(0, "Kg");
    }

    partial void OnNewExerciseNameChanged(string value)
    {
        if (value is null)
            return;

        var existingExercice = Exercises
            .FirstOrDefault(e => e.ExerciseName.Equals(value));

        if (existingExercice is null)
        {
            ExistingExerciseNameInError = false;
            SelectedExercise = null;
            CurrentExercise = new Exercise()
            {
                ExerciseName = value,
                Id = Guid.NewGuid(),
                ExerciseDifficulties = []
            };
        }
        else
        {
            ExistingExerciseNameInError = true;
        }
    }


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

            await Repository.LikeUpdateAsync(exerciseInDatabase);
            await Repository.SaveChangesAsync(CancellationToken.None);
        }
        else
        {
            //nouvel exercice

            //on veut en créer un avec un nom existant? refusé
            var exerciseWithSameNameInDatabase = Repository.Query<Exercise>()
                .Where(e => e.ExerciseName.Equals(CurrentExercise.ExerciseName))
                .Include(e => e.ExerciseDifficulties)
                .FirstOrDefault();

            if (exerciseWithSameNameInDatabase is not null)
                return;

            //autre exercice, on lui ajoute sa difficulté
            CurrentExercise.ExerciseDifficulties.Add(CurrentDifficulty);
            await Repository.AddAsync(CurrentExercise);
            await Repository.SaveChangesAsync(CancellationToken.None);
            await LoadExercises();
        }

        CurrentDifficulty = new(0, "Kg");
    }

    [RelayCommand]
    public async Task Back()
    {
        await Navigation.NavigateBack();
    }

    [RelayCommand]
    public async Task LoadExercises()
    {
        Exercises = new ObservableCollection<Exercise>(Repository.Query<Exercise>()
            .Include(e => e.ExerciseDifficulties)
            .ToList());
        SelectedExercise = null;
        NewExerciseName = string.Empty;
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