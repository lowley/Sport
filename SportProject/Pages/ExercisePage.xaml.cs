using Serilog.Core;
using Sport.VM;
using SportProject.Platforms.Android;

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

    private void DifficultyLevelEntry_Focused(object sender, FocusEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            DifficultyLevelEntry.Text = "";
        });
    }

    private void HideKeyboard_Clicked(object sender, EventArgs e)
    {
        KeyboardHelper.HideKeyboard();
    }
}