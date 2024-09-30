using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace ClientUtilsProject.ViewModels;

public partial class SessionVM : ObservableObject
{
    [ObservableProperty] private Session _session;

    [ObservableProperty]
    public ObservableCollection<Exercise> _exercises = [];

    [ObservableProperty] public Exercise _selectedExercise;

    [NotifyPropertyChangedFor(nameof(AvailableDifficultiesText))]
    [ObservableProperty] public ExerciceDifficulty _selectedDifficulty;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(RepetitionTotal))]
    public int _repetitions = 10;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RepetitionTotal))]
    public int _repetitionAdjustment = 0;
    
    [ObservableProperty]
    public int _oneSessionExerciceSerieSelectedIndex = -1;
    
    public int RepetitionTotal => Repetitions + RepetitionAdjustment;
    
    public string AvailableDifficultiesText
        => $"Difficultés pour {SelectedExercise?.ExerciseName}";

    public ISportRepository Repository { get; set; }
    private SportContext Context { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }

    [RelayCommand]
    public async Task AddOneSerie()
    {
        //SelectedExercise, SelectedDifficulty, Repetitions
        var foundExerciseSeries = Session.SessionItems
            .LastOrDefault();

        if (foundExerciseSeries?.ExerciceId == SelectedExercise.Id &&
            foundExerciseSeries?.DifficultyId == _selectedDifficulty.Id &&
            foundExerciseSeries?.Repetitions == RepetitionTotal)
        { }
        else
            foundExerciseSeries = null;

        if (foundExerciseSeries == null)
        {
            var sessionToAdd = new SessionExerciceSerie()
            {
                ExerciceId = SelectedExercise.Id,
                Exercice = SelectedExercise,
                DifficultyId = SelectedDifficulty.Id,
                Difficulty = SelectedDifficulty,
                SessionId = Session.Id,
                Session = Session,
                Repetitions = RepetitionTotal,
                Series = 1
            };
            
            Session.SessionItems.Add(sessionToAdd);
            Session.ModifySessionItems();
            Repository.GetContext().Entry(sessionToAdd).State = EntityState.Added;
        }
        else
        {
            foundExerciseSeries.Series += 1;
        }
        
        Session.ModifySessionItems();
    }

    [RelayCommand]
    public void RepetitionsMinus1()
    {
        Repetitions -= 1;
    }
    
    [RelayCommand]
    public void RepetitionsPlus1()
    {
        Repetitions += 1;
    }

    [RelayCommand]
    public void SetNumberRepetitions(string number)
    {
        Repetitions = int.Parse(number);
        RepetitionAdjustment = 0;
    }
    
    [RelayCommand]
    public async Task CloseSession()
    {
        Session.SessionEndTime = DateTime.Now.TimeOfDay;
        Session.IsOpened = false;

        if (Session.SessionItems.Any())
        {
            foreach (var session in Repository.GetContext().Sessions.Local)
            {
                var asdfgdsfgd = Repository.GetContext().Entry(session).State.ToString();
            }

            await Repository.SaveChangesAsync(CancellationToken.None);
        }
        
        await Shell.Current.Navigation.PopAsync();
    }
    
    [RelayCommand]
    public async Task ExitKeepSession()
    {
        if (Session.SessionItems.Any())
        {
            await Repository.SaveChangesAsync(CancellationToken.None);
        }
        
        await Navigation.NavigateBack();
    }

    public SessionVM(
        ISportNavigation navigation, 
        SportContext context,
        ISportLogger logger,
        ISportRepository repository)
    {
        Navigation = navigation;
        Repository = repository;
        Context = context;
        Logger = logger;
    }

    public void DeleteSessionExerciceSerie(Guid sessionExerciceSerieId)
    {
        var sesToRemove = Session.SessionItems
            .FirstOrDefault(ses => ses.Id == sessionExerciceSerieId);

        //bug théorique dans ce cas
        if (sesToRemove == null)
            return;

        Session.SessionItems.Remove(sesToRemove);
        Session.ModifySessionItems();
    }
}