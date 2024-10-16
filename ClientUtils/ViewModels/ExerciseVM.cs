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
    //des exercices
    [ObservableProperty] public ObservableCollection<Exercise> _exercises = [];

    //un exercice
    [ObservableProperty] public Exercise? _selectedExercise;

    //une difficulté
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(CurrentDifficulty))]
    public ExerciceDifficulty _selectedDifficulty;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(CurrentDifficulty))]
    public ExerciceDifficulty? _potentialNewDifficulty;

    public ExerciceDifficulty? CurrentDifficulty
        => SelectedDifficulty ?? PotentialNewDifficulty;

    //majors
    [ObservableProperty] public string _existingExerciseName;
    [ObservableProperty] public string _newExerciseName;

    //minors
    [ObservableProperty] public bool _editingSelectedExercise;
    [ObservableProperty] public bool _editingNewExerciseName;
    [ObservableProperty] public bool _existingExerciseNameInError;
    [ObservableProperty] public bool _anyExerciseTappedState;
    public Guid? LastSelectedExerciseId;
    [ObservableProperty] public bool _anyDifficultyTappedState;
    public Guid? LastSelectedDifficultyId;

    private ISportRepository Repository { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }

    // partial void OnSelectedExerciseChanged(Exercise value)
    // {
    //     if (value is null || EditingNewExerciseName)
    //     {
    //         CurrentExercise = new();
    //     }
    //     else
    //     {
    //         CurrentExercise = value;
    //
    //         EditingSelectedExercise = true;
    //         NewExerciseName = string.Empty;
    //         EditingSelectedExercise = false;
    //     }
    //
    //     CurrentDifficulty = new ExerciceDifficulty(0, "Kg");
    // }

    // partial void OnNewExerciseNameChanged(string value)
    // {
    //     if (value is null || EditingSelectedExercise)
    //         return;
    //
    //     var existingExercice = Exercises
    //         .FirstOrDefault(e => e.ExerciseName.Equals(value));
    //
    //     if (existingExercice is null)
    //     {
    //         ExistingExerciseNameInError = false;
    //
    //         EditingNewExerciseName = true;
    //         SelectedExercise = null;
    //         EditingNewExerciseName = false;
    //
    //         CurrentExercise = new Exercise()
    //         {
    //             ExerciseName = value,
    //             Id = Guid.NewGuid(),
    //             ExerciseDifficulties = []
    //         };
    //     }
    //     else
    //     {
    //         ExistingExerciseNameInError = true;
    //     }
    // }

    // [RelayCommand]
    // public async Task ExistingDifficultyTapped(Guid id)
    // {
    //     if (CurrentDifficulty.DifficultyLevel != 0)
    //     {
    //         CurrentDifficulty = new();
    //         return;
    //     }
    //     
    //     var difficulty = SelectedExercise.ExerciseDifficulties
    //         .FirstOrDefault(d => d.Id == id);
    //
    //     if (difficulty is null)
    //         return;
    //
    //     CurrentDifficulty = new()
    //     {
    //         Id = difficulty.Id,
    //         DifficultyName = difficulty.DifficultyName,
    //         DifficultyLevel = difficulty.DifficultyLevel,
    //         Exercice = difficulty.Exercice,
    //         ExerciceId = difficulty.ExerciceId
    //     };
    //
    //     Repository.GetContext().Entry(CurrentDifficulty).State = EntityState.Unchanged;
    // }

    [RelayCommand]
    public async Task ExerciseTapped(Exercise exercise)
    {
        if (AnyExerciseTappedState && LastSelectedExerciseId == exercise?.Id)
        {
            NewExerciseName = string.Empty;
            ExistingExerciseName = string.Empty;
            SelectedExercise = null;
            AnyExerciseTappedState = !AnyExerciseTappedState;
        }
        else if (!AnyExerciseTappedState)
            AnyExerciseTappedState = true;

        LastSelectedExerciseId = exercise?.Id;
    }

    [RelayCommand]
    public async Task SetName(string newExistingExerciseName)
    {
        NewExerciseName = string.Empty;
        ExistingExerciseName = string.Empty;

        if (string.IsNullOrEmpty(newExistingExerciseName))
            return;

        if (Exercises.Select(e => e.ExerciseName).Contains(newExistingExerciseName))
        {
            //toast d'info de nom existant
            newExistingExerciseName = string.Empty;
            return;
        }
        
        //code commun aux 2 cas
        
        //SelectedExercise ne peut pas être nul par construction du XAML normalement
        if (SelectedExercise is not null)
            SelectedExercise.ExerciseName = newExistingExerciseName;

        //cas spécifique du nouvel exercice
        
        if (SelectedExercise.Id != Exercises.FirstOrDefault()?.Id)
            return;

        var potentialNewExercise = new Exercise();
        var potentialNewDifficulty = new ExerciceDifficulty();
        potentialNewExercise.ExerciseDifficulties.Add(potentialNewDifficulty);
        Exercises.Insert(0, potentialNewExercise);
        Repository.GetContext().Entry(potentialNewDifficulty).State = EntityState.Added;
        Repository.GetContext().Entry(potentialNewExercise).State = EntityState.Added;

        SelectedExercise.RaisePropertyChanged(nameof(Exercise.ExerciseName));
    }

    [RelayCommand]
    public async Task DifficultyTapped(ExerciceDifficulty difficulty)
    {
        if (AnyDifficultyTappedState && LastSelectedDifficultyId == difficulty?.Id
                                     && difficulty?.Id != SelectedExercise?.ExerciseDifficulties[0].Id)
        {
            SelectedDifficulty = null;
            AnyDifficultyTappedState = !AnyDifficultyTappedState;
        }
        else if (!AnyDifficultyTappedState)
            AnyDifficultyTappedState = true;

        LastSelectedDifficultyId = difficulty?.Id;

        if (SelectedDifficulty.Id != SelectedExercise.ExerciseDifficulties[0].Id)
            return;

        SelectedDifficulty.DifficultyLevel = 10;
        SelectedDifficulty.DifficultyName = "Kg";

        var newDifficulty = new ExerciceDifficulty();
        SelectedExercise.ExerciseDifficulties.Insert(0, newDifficulty);
        Repository.GetContext().Entry(newDifficulty).State = EntityState.Added;
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
        var newExercise = Exercises.FirstOrDefault();

        //bug théorique
        if (newExercise is null)
            return;

        var newExerciseState = Repository.GetContext()
            .Entry(newExercise).State;

        //premier exercice non modifié => pas d'ajout => suppression car inutile
        if (newExerciseState == EntityState.Added)
        {
            Exercises.RemoveAt(0);
            Repository.GetContext().Entry(newExercise).State = EntityState.Detached;
        }

        foreach (var exercise in Exercises)
        {
            var newDifficulty = exercise.ExerciseDifficulties.FirstOrDefault();
            //bug théorique
            if (newDifficulty is null)
                return;
            var newDifficultyState = Repository.GetContext()
                .Entry(newDifficulty).State;
            if (newDifficultyState == EntityState.Added)
            {
                exercise.ExerciseDifficulties.RemoveAt(0);
                Repository.GetContext().Entry(newDifficulty).State = EntityState.Detached;
            }
        }

        await Repository.SaveChangesAsync();
        await LoadExercises();

        // Guid recoveryId = Guid.Empty;
        //
        // if (IsCreationExercise())
        // {
        //     var savedNewExercise = await CreateExerciseAsync();
        //     recoveryId = savedNewExercise?.Id ?? Guid.Empty;
        //
        //     //the exercise is created
        //     //because CreationExercise tests if datas are valid
        //     if (IsCreationDifficulty() && savedNewExercise is not null)
        //         await CreateDifficulty(savedNewExercise);
        // }
        // else
        // {
        //     if (IsModificationExercise())
        //     {
        //         Exercise exercise = CurrentExercise;
        //
        //         if (IsModificationExerciseName())
        //             exercise = await ModifyExerciseName();
        //
        //         recoveryId = exercise.Id;
        //
        //         if (IsModificationDifficulty())
        //             await ModifyDifficulty(exercise);
        //         else if (IsCreationDifficulty())
        //             await CreateDifficulty(exercise);
        //     }
        //
        //     await Repository.SaveChangesAsync();
        // }
        //
        // bool IsCreationExercise()
        // {
        //     var bad = string.IsNullOrEmpty(NewExerciseName)
        //               || Exercises.Any(e => e.ExerciseName == NewExerciseName);
        //     return !bad;
        // }
        //
        // bool IsCreationDifficulty()
        // {
        //     var result = CurrentDifficulty is not null
        //                  && CurrentDifficulty.DifficultyLevel > 0
        //                  && CurrentDifficulty.DifficultyName.IsSome;
        //     return result;
        // }
        //
        // bool IsModificationExercise()
        // {
        //     var result = Exercises.Any(e => e.Id == CurrentExercise.Id);
        //     return result;
        // }
        //
        // bool IsModificationExerciseName()
        // {
        //     var result = !string.IsNullOrEmpty(ExistingExerciseName);
        //     return result;
        // }
        //
        // bool IsModificationDifficulty()
        // {
        //     var result = CurrentDifficulty is not null
        //                  && CurrentExercise.ExerciseDifficulties
        //                      .Any(d => d.Id == CurrentDifficulty.Id)
        //                  && CurrentDifficulty.DifficultyLevel > 0
        //                  && CurrentDifficulty.DifficultyName.IsSome;
        //
        //     return result;
        // }
        //
        // async Task<Exercise> CreateExerciseAsync()
        // {
        //     var newExercise = new Exercise()
        //     {
        //         Id = Guid.NewGuid(),
        //         ExerciseName = NewExerciseName,
        //         ExerciseDifficulties = []
        //     };
        //
        //     var savedExercise = await Repository.AddAsync(newExercise);
        //     await Repository.SaveChangesAsync(CancellationToken.None);
        //     await Repository.ReloadAsync();
        //
        //     var state = Repository.GetContext()?.Entry(savedExercise).State;
        //     if (state is not null)
        //         state = EntityState.Unchanged;
        //
        //     foreach (var d in savedExercise.ExerciseDifficulties)
        //     {
        //         state = Repository.GetContext()?.Entry(d).State;
        //         if (state is not null)
        //             state = EntityState.Detached;
        //     }
        //
        //     return savedExercise;
        // }
        //
        // async Task<ExerciceDifficulty> CreateDifficulty(Exercise savedNewExercise)
        // {
        //     CurrentDifficulty.ExerciceId = savedNewExercise.Id;
        //     CurrentDifficulty.Exercice = null;
        //     var savedDifficulty = await Repository.AddAsync(CurrentDifficulty);
        //
        //     await Repository.SaveChangesAsync(CancellationToken.None);
        //     return savedDifficulty;
        // }
        //
        // async Task<Exercise> ModifyExerciseName()
        // {
        //     Repository.Attach(CurrentExercise);
        //     CurrentExercise.ExerciseName = ExistingExerciseName;
        //     await Repository.SaveChangesAsync();
        //     await Repository.ReloadAsync();
        //     CurrentExercise = Repository.Query<Exercise>()
        //         .Include(e => e.ExerciseDifficulties)
        //         .AsNoTracking()
        //         .FirstOrDefault(e => e.Id == CurrentExercise.Id);
        //
        //     if (CurrentExercise is null)
        //         return null;
        //
        //     return CurrentExercise;
        // }
        //
        // async Task ModifyDifficulty(Exercise exercise)
        // {
        //     Repository.Attach(CurrentDifficulty);
        //     CurrentDifficulty.Exercice = exercise;
        //     await Repository.SaveChangesAsync(CancellationToken.None);
        //     await Repository.ReloadAsync();
        //     CurrentDifficulty = Repository.Query<ExerciceDifficulty>()
        //         .Include(d => d.Exercice)
        //         .AsNoTracking()
        //         .FirstOrDefault(e => e.Id == CurrentDifficulty.Id);
        // }
        //
        // await LoadExercises();
        // CurrentDifficulty = new(0, "Kg");
        // SelectedExercise = Exercises
        //     .FirstOrDefault(e => e.Id == recoveryId);
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
        Exercises.Clear();

        var potentialNewExercise = new Exercise();
        Repository.GetContext().Entry(potentialNewExercise).State = EntityState.Added;
        PotentialNewDifficulty = new();
        potentialNewExercise.ExerciseDifficulties.Insert(0, PotentialNewDifficulty);
        Repository.GetContext().Entry(PotentialNewDifficulty).State = EntityState.Added;

        Exercises.Add(potentialNewExercise);
        var newExercises = Repository.Query<Exercise>()
            .Include(e => e.ExerciseDifficulties)
            .ToList();
        newExercises.ForEach(e =>
        {
            var eNewDifficulty = new ExerciceDifficulty();
            Repository.GetContext().Entry(eNewDifficulty).State = EntityState.Added;
            e.ExerciseDifficulties.Insert(0, eNewDifficulty);
            Exercises.Add(e);
        });


        SelectedDifficulty = null;
        SelectedExercise = null;
        NewExerciseName = string.Empty;
        ExistingExerciseName = string.Empty;

        // var trackedEntities = Repository.GetContext().ExerciceDifficulties.Local;
        // Console.WriteLine(trackedEntities.Count);
    }

    public ExerciseVM(
        ISportNavigation navigation,
        ISportRepository repository,
        ISportLogger logger)
    {
        Navigation = navigation;
        Repository = repository;
        Logger = logger;
    }
}