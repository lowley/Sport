using Serilog.Core;
using Sport.VM;

namespace Sport.Pages;

public partial class ExercisePage : ContentPage
{
    public ExerciseVM VM { get; set; }
    private Logger Logger { get; set; }

    public ExercisePage(ExerciseVM vm, Logger logger)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }
}