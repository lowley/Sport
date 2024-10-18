using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace ClientUtilsProject.ViewModels;

public partial class SessionsVM : ObservableObject
{
    [ObservableProperty] public ObservableCollection<Session> _sessions = [];

    private SportContext Context { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }
    public ISportRepository Repository { get; set; }

    [RelayCommand]
    public async Task SetSessionAsActive(Session toActive)
    {
        foreach (var session in Sessions)
            session.IsOpened = false;

        toActive.IsOpened = true;
        await Repository.SaveChangesAsync();
    }
    
    [RelayCommand]
    public async Task NavigateToSessionPage(Guid sessionId)
    {
        // var session = VM.Sessions.FirstOrDefault(s => s.Id == sessionId);
        //
        // //bug théorique
        // if (session == null)
        //     return;

        var param = new Dictionary<string, object>
        {
            {"sessionId", sessionId.ToString() },
        };

        await Shell.Current.GoToAsync("sessions/session", false, param);
    }

    [RelayCommand]
    public async Task PropChangeAsync()
    {
        foreach (var session in Sessions)
        {
            session.RaisePropertyChanged(nameof(Session.IsOpened));
            session.RaisePropertyChanged(nameof(Session.SessionStartDate));
            session.RaisePropertyChanged(nameof(Session.SessionStartTime));
            session.RaisePropertyChanged(nameof(Session.SessionEndTime));
        }
    }
    
    public SessionsVM(
        ISportNavigation navigation, 
        SportContext context,
        ISportLogger logger,
        ISportRepository repo)
    {
        Navigation = navigation;
        Context = context;
        Logger = logger;
        Repository = repo;
        Sessions = [];
    }

    public void RaiseSessionsChanged()
    {
        OnPropertyChanged(nameof(Sessions));
        
    }
}