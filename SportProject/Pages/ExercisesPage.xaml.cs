using Serilog.Core;
using ClientUtilsProject.ViewModels;

namespace Sport.Pages;

public partial class ExercisesPage : ContentPage
{
    public ExercisesVM VM { get; set; }
    private Logger Logger { get; set; }

    public ExercisesPage(ExercisesVM vm, Logger logger)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }
}