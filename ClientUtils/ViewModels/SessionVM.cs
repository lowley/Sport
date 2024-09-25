using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientUtilsProject.ViewModels;

public partial class SessionVM : ObservableObject
{
    [ObservableProperty] private Session _session;

    [ObservableProperty]
    public ObservableCollection<Exercise> _exercises = [];

    [ObservableProperty] public Exercise _selectedExercise;

    [NotifyPropertyChangedFor(nameof(AvailableDifficultiesText))]
    [ObservableProperty] public ExerciceDifficulty _selectedDifficulty;

    [ObservableProperty] public int _repetitions = 10;
    
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
            .FirstOrDefault(ses =>
                ses.ExerciceId == SelectedExercise.Id &&
                ses.DifficultyId == _selectedDifficulty.Id &&
                ses.Repetitions == Repetitions);

        if (foundExerciseSeries == null)
        {
            Session.SessionItems.Add(new SessionExerciceSerie(){
                ExerciceId = SelectedExercise.Id,
                Exercice = SelectedExercise,
                DifficultyId = SelectedDifficulty.Id,
                Difficulty = SelectedDifficulty,
                SessionId = Session.Id,
                Session = Session,
                Repetitions = Repetitions,
                Series = 1
            });
        }
        else
        {
            foundExerciseSeries.Series += 1;
        }
        
        OnPropertyChanged(nameof(Session.SessionItems));
        OnPropertyChanged(nameof(Session.GroupedSessionItems));
        foreach (var item in Session.GroupedSessionItems)
        {
            OnPropertyChanged(nameof(Grouping.Key));
            OnPropertyChanged(nameof(Grouping.Group));
        }
    }

    [RelayCommand]
    public async Task CloseSession()
    {
        Session.SessionEndTime = DateTime.Now.TimeOfDay;

        if (Session.SessionItems.Any())
        {
            Repository.SaveChangesAsync(CancellationToken.None);
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
        
        Session = new Session();
        Repository.Attach(Session);
    }
}