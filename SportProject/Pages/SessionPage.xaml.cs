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
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(VM.Session.SessionItems));
        OnPropertyChanged(nameof(VM.Session.GroupedSessionItems));

        VM.Session.GroupedSessionItems.RaisePropertyChanged("CollectionOfGroupings");
        VM.Session.GroupedSessionItems.RaisePropertyChanged("GroupingsCount");
        VM.Session.GroupedSessionItems.RaisePropertyChanged("Key");
        GroupedSessionItemsType.Instance.CollectionOfGroupings.ForEach(g =>
        {
            g.RaisePropertyChanged("Key");
            g.RaisePropertyChanged("Group");
        });
        
        Trace.WriteLine("test");
    }
}