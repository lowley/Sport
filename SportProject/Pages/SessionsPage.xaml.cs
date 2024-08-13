using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using ClientUtilsProject.ViewModels;

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
    
    public SessionsPage()
    {
        InitializeComponent();
    }
}