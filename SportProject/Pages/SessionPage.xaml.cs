using Serilog.Core;
using ClientUtilsProject.ViewModels;

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
}