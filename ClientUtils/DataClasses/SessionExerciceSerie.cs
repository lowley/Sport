using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Net.Mime;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Buttons;
using Microsoft.Maui.Graphics;
using Font = System.Drawing.Font;
using SizeF = Microsoft.Maui.Graphics.SizeF;

namespace ClientUtilsProject.DataClasses;

public partial class SessionExerciceSerie : SportEntity
{
    public Guid SessionId { get; set; }
    [ObservableProperty]
    public Session _session;
    
    public Guid ExerciceId { get; set; }
    [ObservableProperty]
    public Exercise _exercice;
    
    public Guid DifficultyId { get; set; }
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowSummary))]
    [NotifyPropertyChangedFor(nameof(SegmentedControlItems))]
    public ExerciceDifficulty _difficulty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowSummary))]
    [NotifyPropertyChangedFor(nameof(SegmentedControlItems))]
    public Int32 _series;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowSummary))]
    [NotifyPropertyChangedFor(nameof(SegmentedControlItems))]
    public Int32 _repetitions;
    
    public string ShowSummary => $"{Difficulty.ShowMeShort}*{Repetitions}:{Series}";
    
    [NotMapped]
    public ObservableCollection<SfSegmentItem> SegmentedControlItems => new ()
    {
        new SfSegmentItem(){Text = Difficulty.ShowMeShort, Width = 70},
        new SfSegmentItem(){Text = Repetitions.ToString(), Width = 60},
        new SfSegmentItem(){Text = Series.ToString(), Width = 60},
    };

    public void RaisePropertyChanged(string property)
    {
        OnPropertyChanged(property);
    }
    
    public SessionExerciceSerie()
    {
        Id = Guid.NewGuid();
        Difficulty = new ExerciceDifficulty();
        DifficultyId = Guid.NewGuid();
        Exercice = new Exercise();
        ExerciceId = Guid.NewGuid();
    }
}