using ClientUtilsProject.ViewModels;
using Serilog.Core;

namespace SportProject.Pages;

public partial class ExercisesPage : ContentPage
{
    public ExercisesVM VM { get; set; }
    private Logger Logger { get; set; }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(async () => await VM.LoadExercises());
    }


    public ExercisesPage(ExercisesVM vm, Logger logger)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }
}