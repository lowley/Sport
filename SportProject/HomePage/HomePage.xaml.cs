using Serilog.Core;
using System.Diagnostics;
using Sport.VM;

namespace Sport.Pages;

public partial class HomePage : ContentPage
{
    public HomeVM VM { get; set; }
    private Logger Logger { get; set; }

    public HomePage(HomeVM vm, Logger logger)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }
}

