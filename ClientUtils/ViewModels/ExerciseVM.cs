﻿using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace ClientUtilsProject.ViewModels;

public partial class ExerciseVM : ObservableObject
{
    //majors
    [ObservableProperty] public Exercise _currentExercise;
    [ObservableProperty] public ExerciceDifficulty _currentDifficulty;
    [ObservableProperty] public string _existingExerciseName;
    [ObservableProperty] public string _newExerciseName;

    //minors
    [ObservableProperty] public Exercise _selectedExercise;
    [ObservableProperty] public bool _editingSelectedExercise;
    [ObservableProperty] public bool _editingNewExerciseName;
    [ObservableProperty] public bool _existingExerciseNameInError;
    [ObservableProperty] public ObservableCollection<Exercise> _exercises;

    private ISportRepository Repository { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }


    partial void OnSelectedExerciseChanged(Exercise value)
    {
        if (value is null || EditingNewExerciseName)
        {
            CurrentExercise = new();
        }
        else
        {
            CurrentExercise = value;

            EditingSelectedExercise = true;
            NewExerciseName = string.Empty;
            EditingSelectedExercise = false;
        }

        CurrentDifficulty = new ExerciceDifficulty(0, "Kg");
    }

    partial void OnNewExerciseNameChanged(string value)
    {
        if (value is null || EditingSelectedExercise)
            return;

        var existingExercice = Exercises
            .FirstOrDefault(e => e.ExerciseName.Equals(value));

        if (existingExercice is null)
        {
            ExistingExerciseNameInError = false;

            EditingNewExerciseName = true;
            SelectedExercise = null;
            EditingNewExerciseName = false;

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
    public async Task ExistingDifficultyTapped(Guid id)
    {
        if (CurrentDifficulty.DifficultyLevel != 0)
        {
            CurrentDifficulty = new();
            return;
        }
        else
        {
            var difficulty = SelectedExercise.ExerciseDifficulties
                .FirstOrDefault(d => d.Id == id);

            if (difficulty is null)
                return;

            CurrentDifficulty = new()
            {
                Id = difficulty.Id,
                DifficultyName = difficulty.DifficultyName,
                DifficultyLevel = difficulty.DifficultyLevel,
                Exercice = difficulty.Exercice,
                ExerciceId = difficulty.ExerciceId
            };
        }

        Task.Run(async () => Repository.LikeUpdateAsync(CurrentDifficulty));
    }

    /**
     * CurrentExercise:
     * - l' exercice sélectionné<br/>
     * - ou NOUVEAU<br/>
     *
     * CurrentDifficulty:<br/>
     * - si un exercice sélectionné<br/>
     * - - si un Chip sélectionné: EXISTANTE<br/>
     * - - si aucun Chip sélectionné: NOUVELLE<br/>
     * - si aucun exercice sélectionné: NOUVELLE<br/>
     */
    [RelayCommand]
    public async Task Save()
    {
        Guid recoveryId = Guid.Empty;
        
        if (IsCreationExercise())
        {
            var savedNewExercise = await CreateExerciseAsync();
            recoveryId = savedNewExercise.Id;

            //the exercise is created
            //because CreationExercise tests if datas are valid
            if (IsCreationDifficulty() && savedNewExercise is not null)
                await CreateDifficulty(savedNewExercise);
        }
        
        else
        {
            if (IsModificationExercise())
            {
                Exercise exercise = CurrentExercise;

                if (IsModificationExerciseName())
                    exercise = await ModifyExerciseName();

                recoveryId = exercise.Id;
                
                if (IsCreationDifficulty())
                    await CreateDifficulty(exercise);
                else if (IsModificationDifficulty())
                    await ModifyDifficulty(exercise);
            }

            await Repository.SaveChangesAsync();
        }

        bool IsCreationExercise()
        {
            var result = !string.IsNullOrEmpty(NewExerciseName);
            return result;
        }

        bool IsCreationDifficulty()
        {
            var result = CurrentDifficulty is not null
                         && CurrentDifficulty.DifficultyLevel > 0
                         && CurrentDifficulty.DifficultyName.IsSome;
            return result;
        }

        bool IsModificationExercise()
        {
            var result = Exercises.Any(e => e.Id == CurrentExercise.Id);
            return result;
        }

        bool IsModificationExerciseName()
        {
            var result = !string.IsNullOrEmpty(ExistingExerciseName);
            return result;
        }

        bool IsModificationDifficulty()
        {
            var result = CurrentDifficulty is not null
                && CurrentExercise.ExerciseDifficulties
                    .Any(d => d.Id == CurrentDifficulty.Id)
                && CurrentDifficulty.DifficultyLevel > 0
                && CurrentDifficulty.DifficultyName.IsSome;

            return result;
        }

        async Task<Exercise> CreateExerciseAsync()
        {
            var newExercise = new Exercise()
            {
                Id = Guid.NewGuid(),
                ExerciseName = NewExerciseName,
                ExerciseDifficulties = []
            };

            await Repository.AddAsync(newExercise);
            await Repository.SaveChangesAsync(CancellationToken.None);
            await Repository.ReloadAsync();
            var savedExercise = Repository.Query<Exercise>()
                .Include(e => e.ExerciseDifficulties)
                .FirstOrDefault(e => e.Id == newExercise.Id);
            
            return savedExercise;
        }

        async Task<ExerciceDifficulty> CreateDifficulty(Exercise savedNewExercise)
        {
            CurrentDifficulty.Exercice = savedNewExercise;
            await Repository.AddAsync(CurrentDifficulty);
            await Repository.SaveChangesAsync(CancellationToken.None);
            await Repository.ReloadAsync();
            var savedDifficulty = Repository.Query<ExerciceDifficulty>()
                .Include(d => d.Exercice)
                .FirstOrDefault(d => d.Id == CurrentDifficulty.Id);
            
            return savedDifficulty;
        }

        async Task<Exercise> ModifyExerciseName()
        {
            CurrentExercise.ExerciseName = ExistingExerciseName;
            await Repository.LikeUpdateAsync(CurrentExercise);
            return CurrentExercise;
        }

        async Task ModifyDifficulty(Exercise exercise)
        {
            CurrentDifficulty.Exercice = exercise;
            await Repository.LikeUpdateAsync(CurrentDifficulty);
        }

        //*********************************

        // var existingExerciseNameEmptyOrSame =
        //     string.IsNullOrEmpty(ExistingExerciseName)
        //     || ExistingExerciseName.Equals(CurrentExercise.ExerciseName);
        //
        // var currentDifficultyZeroOrNewOrSame =
        //     CurrentDifficulty.DifficultyLevel == 0
        //     || (CurrentDifficulty.DifficultyLevel ==
        //         (CurrentExercise.ExerciseDifficulties.FirstOrDefault(d => d.Id == CurrentDifficulty.Id)
        //             ?.DifficultyLevel ?? -1)
        //         && CurrentDifficulty.DifficultyName.Equals(
        //             CurrentExercise.ExerciseDifficulties.FirstOrDefault(d => d.Id == CurrentDifficulty.Id)
        //                 ?.DifficultyName ?? "µ*ù"
        //         ));
        //
        // if (CurrentExercise is not null
        //     && existingExerciseNameEmptyOrSame && currentDifficultyZeroOrNewOrSame)
        //     return;
        //
        // var exerciseInDatabase = Repository.Query<Exercise>()
        //     .Where(e => e.Id == CurrentExercise.Id)
        //     .Include(e => e.ExerciseDifficulties)
        //     .FirstOrDefault();
        //
        // // if (exerciseInDatabase is not null &&
        // //     exerciseInDatabase.ExerciseName.Equals(CurrentExercise.ExerciseName) &&
        // //     exerciseInDatabase.ExerciseDifficulties.Any(oneDifficulty =>
        // //         oneDifficulty == CurrentDifficulty))
        // //     return;
        //
        // var recoveryExerciseId = Guid.Empty;
        //
        // if (exerciseInDatabase is not null)
        // {
        //     recoveryExerciseId = exerciseInDatabase.Id;
        //     //l'exercice existe déjà
        //     // changement de nom?
        //     if (!ExistingExerciseName.Equals(exerciseInDatabase.ExerciseName)
        //         && !string.IsNullOrEmpty(ExistingExerciseName)
        //         && !Exercises.Any(e => e.ExerciseName.Equals(ExistingExerciseName)))
        //         exerciseInDatabase.ExerciseName = ExistingExerciseName;
        //
        //     // ajout de difficulté?
        //     if (exerciseInDatabase.ExerciseDifficulties.All(diff => diff.Id != CurrentDifficulty.Id)
        //         && CurrentDifficulty.DifficultyLevel != 0)
        //         exerciseInDatabase.ExerciseDifficulties.Add(CurrentDifficulty);
        //
        //     await Repository.LikeUpdateAsync(exerciseInDatabase);
        //     await Repository.SaveChangesAsync(CancellationToken.None);
        // }
        // else
        // {
        //     //nouvel exercice
        //
        //     //on veut en créer un avec un nom existant? refusé
        //     var exerciseWithSameNameInDatabase = Repository.Query<Exercise>()
        //         .Where(e => e.ExerciseName.Equals(NewExerciseName))
        //         .Include(e => e.ExerciseDifficulties)
        //         .FirstOrDefault();
        //
        //     if (exerciseWithSameNameInDatabase is not null)
        //         return;
        //
        //     recoveryExerciseId = CurrentExercise.Id;
        //
        //     //autre exercice, on lui ajoute sa difficulté
        //     Exercise newExercise = new()
        //     {
        //         Id = CurrentExercise.Id,
        //         ExerciseName = NewExerciseName,
        //         ExerciseDifficulties = new()
        //     };
        //     if (CurrentDifficulty.DifficultyLevel != 0)
        //         newExercise.ExerciseDifficulties.Add(CurrentDifficulty);
        //
        //     await Repository.AddAsync(newExercise);
        //     await Repository.SaveChangesAsync(CancellationToken.None);
        // }

        await LoadExercises();
        CurrentDifficulty = new(0, "Kg");
        SelectedExercise = Exercises
            .FirstOrDefault(e => e.Id == recoveryId);
    }

    [RelayCommand]
    public async Task Back()
    {
        await Navigation.NavigateBack();
    }

    [RelayCommand]
    public async Task LoadExercises()
    {
        await Repository.ReloadAsync();

        Exercises = new ObservableCollection<Exercise>(Repository.Query<Exercise>()
            .Include(e => e.ExerciseDifficulties)
            .ToList());
        SelectedExercise = null;
        NewExerciseName = string.Empty;
        ExistingExerciseName = string.Empty;
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