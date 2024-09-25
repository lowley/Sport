using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class Session : SportEntity
{
    [ObservableProperty]
    public DateTime _sessionStartDate = DateTime.Now;

    [ObservableProperty]
    public TimeSpan _sessionStartTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    public TimeSpan _sessionEndTime = DateTime.Now.TimeOfDay;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(GroupedSessionItems))]
    public ObservableCollection<SessionExerciceSerie> _sessionItems = [];

    [NotMapped] 
    [ObservableProperty] 
    public ObservableCollection<Grouping> _groupedSessionItems = [];

    partial void OnSessionItemsChanged(ObservableCollection<SessionExerciceSerie> value)
    { }

    private ObservableCollection<Grouping> CreateCollection(IEnumerable<IGrouping<string,SessionExerciceSerie>> collection)
    {
        var result = new ObservableCollection<Grouping>();
        collection.Select(g => new Grouping(g)).ToList().ForEach(g =>
        {
            if (g.Group.Any())
                result.Add(g);
        });
        return result;
    }

    public Session()
    {
        Id = Guid.NewGuid();
        _sessionItems = [];

        PropertyChanged += (sender, args) =>
        {
            Trace.WriteLine($"Session: {args.PropertyName}");
            foreach (var g in GroupedSessionItems)
            {
                g.RaisePropertyChanged("Key"); 
            }
        };

        SessionItems.CollectionChanged += (sender, args) =>
        {
            var group = SessionItems.GroupBy(si => si.Exercice.ExerciseName);
        
            GroupedSessionItems.Clear();
            group.Select(g => new Grouping(g)).ToList().ForEach(g =>
            {
                GroupedSessionItems.Add(g);
            });
            //OnPropertyChanged(nameof(GroupedSessionItems));
            
            
        };
    }
}

[INotifyPropertyChanged]
public partial class Grouping
{
    [ObservableProperty] public ObservableCollection<SessionExerciceSerie> _group = [];

    [ObservableProperty] public string _key = string.Empty;
    
    public Grouping(System.Linq.IGrouping<string, SessionExerciceSerie> grouping)
    {
        List<SessionExerciceSerie> liste = [];
        
        Group.Clear();
        grouping.ToList().ForEach(ses => Group.Add(ses));
        Key = liste.Any() ? grouping.Key : "";
    }

    public void RaisePropertyChanged(string property)
    { 
        OnPropertyChanged(property);    
    }
}

