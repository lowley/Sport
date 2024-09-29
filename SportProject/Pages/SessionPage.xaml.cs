using System.Collections.ObjectModel;
using System.Diagnostics;
using ClientUtilsProject.DataClasses;
using Serilog.Core;
using ClientUtilsProject.ViewModels;
using DevExpress.Maui.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Maui.Buttons;
using SelectionChangedEventArgs = Syncfusion.Maui.Buttons.SelectionChangedEventArgs;

namespace Sport.Pages;

[QueryProperty(nameof(SessionId), "sessionId")]
public partial class SessionPage : ContentPage
{
    public SessionVM VM { get; set; }
    private Logger Logger { get; set; }
    public string SessionId { get; set; }

    public SessionPage(SessionVM vm, Logger logger)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        VM.Exercises.Clear();
        VM.Repository.Query<Exercise>()
            .AsNoTracking()
            .Include(e => e.ExerciseDifficulties)
            .ToList()
            .ForEach(e => VM.Exercises.Add(e));

        if (SessionId != null)
        {
            VM.Session = VM.Repository.Query<Session>()
                .Include(s => s.SessionItems)
                .ThenInclude(si => si.Difficulty)
                .Include(s => s.SessionItems)
                .ThenInclude(si => si.Exercice)
                .FirstOrDefault(s => s.Id == Guid.Parse(SessionId));
        }
        else
            VM.Session = VM.Repository.Query<Session>()
                .Include(s => s.SessionItems)
                .ThenInclude(si => si.Difficulty)
                .Include(s => s.SessionItems)
                .ThenInclude(si => si.Exercice)
                .FirstOrDefault(s => s.IsOpened);


        // var sis = VM.Session.SessionItems;
        // VM.Session.SessionItems.Clear();
        // sis.ForEach(si => VM.Session.SessionItems.Add(si));

        if (VM.Session != null)
        {
            VM.Session.ModifySessionItems();
            VM.Repository.GetContext().Entry(VM.Session).State = EntityState.Unchanged;
        }
        else
        {
            VM.Session = new Session() { IsOpened = true };
            VM.Repository.GetContext().Entry(VM.Session).State = EntityState.Added;
        }
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(VM.Session.SessionItems));
        OnPropertyChanged(nameof(VM.Session.GroupedSessionItems));
    }

    private void SfSegmentedControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var newValue = e.NewValue;
        if (newValue != null && newValue.Text.Equals("-"))
        {
            //supprimer
            var segmentedControl = sender as SfSegmentedControl;
            var source = segmentedControl.ItemsSource as ObservableCollection<SfSegmentItem>;
            var SessionExerciceSerieId = Guid.Parse(source[5].Text);
            VM.DeleteSessionExerciceSerie(SessionExerciceSerieId);
            segmentedControl.SelectedIndex = null;
        }
        else if (newValue != null && newValue.Text.Equals("\u2207"))
        {
            //modifier
            var segmentedControl = sender as SfSegmentedControl;
            var source = segmentedControl.ItemsSource as ObservableCollection<SfSegmentItem>;
            var sessionExerciceSerieId = Guid.Parse(source[5].Text);

            var sessionExerciceSerie = VM.Session.SessionItems
                .FirstOrDefault(s => s.Id == sessionExerciceSerieId);

            if (sessionExerciceSerie == null)
                return;

            VM.SelectedExercise = VM.Exercises.FirstOrDefault(e => e.Id == sessionExerciceSerie.ExerciceId); 
            VM.SelectedDifficulty = VM.SelectedExercise.ExerciseDifficulties.FirstOrDefault(d => d.Id == sessionExerciceSerie.Difficulty.Id);
            VM.Repetitions = sessionExerciceSerie.Repetitions;
            VM.RepetitionAdjustment = 0;
            source[0].Text = "->";
            segmentedControl.SelectedIndex = null;
        }
        else if (newValue != null && newValue.Text.Equals("->"))
        {
            //modifier
            var segmentedControl = sender as SfSegmentedControl;
            var source = segmentedControl.ItemsSource as ObservableCollection<SfSegmentItem>;
            var sessionExerciceSerieId = Guid.Parse(source[5].Text);

            var sessionExerciceSerie = VM.Session.SessionItems
                .FirstOrDefault(s => s.Id == sessionExerciceSerieId);

            if (sessionExerciceSerie == null)
                return;

            sessionExerciceSerie.Exercice = VM.SelectedExercise;
            sessionExerciceSerie.Difficulty = VM.SelectedDifficulty;
            sessionExerciceSerie.Repetitions = VM.RepetitionTotal;
            
            VM.Session.ModifySessionItems();
            
            source[0].Text = "\u2207";
            segmentedControl.SelectedIndex = null;

            VM.Repetitions = 0;
            VM.RepetitionAdjustment = 0;
            VM.SelectedDifficulty = null;
            VM.SelectedExercise = null;
        }
    }
}