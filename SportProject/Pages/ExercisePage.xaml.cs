using System.ComponentModel;
using ClientUtilsProject.DataClasses;
using Serilog.Core;
using ClientUtilsProject.ViewModels;
using SportProject.Platforms.Android;
using Syncfusion.Maui.DataSource.Extensions;

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

        VM.PropertyChanged += async (sender, args) =>
        {
            if (args.PropertyName.Equals(nameof(VM.DifficultiesChanged_ForBackEnd)))
            {
                if (VM.SelectedExercise is null || VM.SelectedExercise.ExerciseDifficulties.Count == 0)
                    return;

                await WaitUntil(() => 
                    VM.SelectedExercise.ExerciseDifficulties.FirstOrDefault()?.Id
                    == DifficultiesChipGroup.ItemsSource?.ToList<ExerciceDifficulty>().FirstOrDefault()?.Id, 30); 
                DifficultiesChipGroup.ItemsSource?.ForEach<ExerciceDifficulty>(d =>
                {
                    DifficultiesChipGroup.GetChipByItem(d)
                        .BackgroundColor = Colors.White;
                });
                var chip = DifficultiesChipGroup.GetChipByItem(VM.SelectedExercise?.ExerciseDifficulties[0]);
                if (chip is not null)
                    chip.BackgroundColor = Colors.Beige;
            }

            if (args.PropertyName.Equals(nameof(VM.ExercicesChanged_ForBackEnd)))
            {
                if (VM.Exercises is null || VM.Exercises.Count == 0)
                    return;

                await WaitUntil(() => 
                    VM.Exercises.FirstOrDefault()?.Id
                    == ExercisesChipGroup.ItemsSource?.ToList<Exercise>().FirstOrDefault()?.Id, 30);
                
                var chip = ExercisesChipGroup.GetChipByItem(VM.Exercises[0]);
                if (chip is not null)
                    chip.BackgroundColor = Colors.Beige;
            }
        };
    }

    private void DifficultyLevelEntry_Focused(object sender, FocusEventArgs e)
    {
        // DifficultyLevelEntry.Unfocus();
        Dispatcher.Dispatch(() => { DifficultyLevelEntry.Value = 0; });
    }

    private void HideKeyboard_Clicked(object sender, EventArgs e)
    {
        //KeyboardHelper.HideKeyboard();
        DifficultiesChipGroup.GetChipByItem(VM.SelectedExercise.ExerciseDifficulties[0])
            .BackgroundColor = Colors.Beige;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        VM.LoadExercises();
    }

    private void Element_OnChildAdded(object? sender, ElementEventArgs e)
    {
        DifficultiesChipGroup.RefreshData();
    }


    public async Task WaitUntil(Func<bool> condition, int checkInterval = 100)
    {
        while (!condition())
            await Task.Delay(checkInterval);
    }
}