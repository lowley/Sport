﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils.SportRepository;
using Serilog.Core;
using ClientUtilsProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Maui.DataSource.Extensions;

namespace SportProject.Pages;

public partial class SessionsPage : ContentPage
{
    public SessionsVM VM { get; set; }

    private Logger Logger { get; set; }

    public SessionsPage(SessionsVM vm, Logger logger)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(async () => await RefreshSessions());
    }

    private async Task RefreshSessions()
    {
        VM.Sessions.Clear();

        await VM.Repository.ReloadAsync();
        (await VM.Repository.Query<Session>()
                .Include(s => s.SessionItems)
                .ThenInclude(ses => ses.Exercice)
                .Include(s => s.SessionItems)
                .ThenInclude(ses => ses.Difficulty)
                .ToListAsync())
            .OrderBy(s => s.SessionStartDate)
            .ThenBy(s => s.SessionStartTime)
            .ForEach(s => VM.Sessions.Add(s));
        foreach (var session in VM.Sessions)
            session?.ModifySessionItems();
        VM.RaiseSessionsChanged();
        await VM.PropChangeAsync();
    }
}