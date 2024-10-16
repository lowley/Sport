using System.Collections.ObjectModel;
using System.ComponentModel;
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
    [ObservableProperty] public bool _difficultiesChanged_ForBackEnd;
    [ObservableProperty] public bool _exercicesChanged_ForBackEnd;
    
    public Guid? LastSelectedDifficultyId;
    public bool DuringDifficultiesChange { get; set; } = false;
    public PropertyChangedEventHandler InSelectedExerciseChangedHandler { get; set; }

    private ISportRepository Repository { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }

    partial void OnSelectedExerciseChanged(Exercise? oldValue, Exercise? newValue)
    {
        //old perd suivi
        //new gagne suivi de sa ExerciseDifficulties
        //qd elle est modifiée, il la trie 
        //redéfinit SelectedExercise après le tri, avec sauvegarde

        if (InSelectedExerciseChangedHandler == null)
            InSelectedExerciseChangedHandler = (object? sender, PropertyChangedEventArgs args) =>
            {
                if (!DuringDifficultiesChange && args.PropertyName.Equals(nameof(Exercise.ExerciseDifficulties)))
                {
                    //tri des difficultés + réassignement SelectedDifficulty
                    var oldSelectedDifficultyId = SelectedDifficulty.Id;
                    DuringDifficultiesChange = true;
                    SelectedExercise.ExerciseDifficulties =
                        new ObservableCollection<ExerciceDifficulty>(
                            SelectedExercise.ExerciseDifficulties.OrderBy(d => d.DifficultyLevel));
                    DuringDifficultiesChange = false;
                    SelectedDifficulty = SelectedExercise.ExerciseDifficulties
                        .FirstOrDefault(d => d.Id == oldSelectedDifficultyId);
                    
                }
            };

        if (oldValue is not null)
            oldValue.PropertyChanged -= InSelectedExerciseChangedHandler;

        if (newValue is not null)
            newValue.PropertyChanged += InSelectedExerciseChangedHandler;
        
        DifficultiesChanged_ForBackEnd = !DifficultiesChanged_ForBackEnd;
    }

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
        DifficultiesChanged_ForBackEnd = !DifficultiesChanged_ForBackEnd;
    }

    [RelayCommand]
    public async Task DifficultyTapped(ExerciceDifficulty difficulty)
    {
        if (AnyDifficultyTappedState && LastSelectedDifficultyId == difficulty?.Id
                                     && difficulty?.Id != SelectedExercise?.ExerciseDifficulties[0].Id)
        {
            SelectedDifficulty = null;
            AnyDifficultyTappedState = !AnyDifficultyTappedState;
            return;
        }
        
        if (!AnyDifficultyTappedState)
            AnyDifficultyTappedState = true;

        LastSelectedDifficultyId = difficulty?.Id;

        if (SelectedDifficulty?.Id != SelectedExercise.ExerciseDifficulties[0].Id)
            return;

        SelectedDifficulty.DifficultyLevel = 10;
        SelectedDifficulty.DifficultyName = "Kg";
        SelectedDifficulty.Exercice = SelectedExercise;

        var collect = SelectedExercise.ExerciseDifficulties.ToList();
        collect = collect.OrderBy(d => d.ShowMeShort).ToList();
        SelectedExercise.ExerciseDifficulties = new(collect);

        var newDifficulty = new ExerciceDifficulty();
        SelectedExercise.ExerciseDifficulties.Insert(0, newDifficulty);
        Repository.GetContext().Entry(newDifficulty).State = EntityState.Added;
        DifficultiesChanged_ForBackEnd = !DifficultiesChanged_ForBackEnd;
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
    }

    [RelayCommand]
    public async Task Back()
    {
        // await Navigation.NavigateBack();

        var temp = SelectedExercise.ExerciseDifficulties[0];
        SelectedExercise.ExerciseDifficulties[0] = SelectedExercise.ExerciseDifficulties[1];
        SelectedExercise.ExerciseDifficulties[1] = temp;

        SelectedExercise.RaisePropertyChanged(nameof(Exercise.ExerciseDifficulties));
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
            e.ExerciseDifficulties =
                new ObservableCollection<ExerciceDifficulty>(e.ExerciseDifficulties.OrderBy(d => d.DifficultyLevel));
            var eNewDifficulty = new ExerciceDifficulty();
            Repository.GetContext().Entry(eNewDifficulty).State = EntityState.Added;
            e.ExerciseDifficulties.Insert(0, eNewDifficulty);
            Exercises.Add(e);
        });

        SelectedDifficulty = null;
        SelectedExercise = null;
        NewExerciseName = string.Empty;
        ExistingExerciseName = string.Empty;
        DifficultiesChanged_ForBackEnd = !DifficultiesChanged_ForBackEnd;
        ExercicesChanged_ForBackEnd = !ExercicesChanged_ForBackEnd;
        
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