using System.Collections.ObjectModel;
using System.Diagnostics;
using ClientUtilsProject.DataClasses;
using Serilog.Core;
using ClientUtilsProject.ViewModels;
using DevExpress.Maui.Core.Internal;
using Microsoft.EntityFrameworkCore;

namespace Sport.Pages;

public partial class SessionPage : ContentPage
{
    public SessionVM VM { get; set; }
    private Logger Logger { get; set; }
    
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
}