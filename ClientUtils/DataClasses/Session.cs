using System.Collections;
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
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(GroupedSessionItems))]
    public ObservableCollection<SessionExerciceSerie> _sessionItems;

    [NotMapped]
    public GroupedSessionItemsType GroupedSessionItems
        => GroupedSessionItemsType.InstanceWithCollection(
            SessionItems.GroupBy(si => si.Exercice.ExerciseName));
    
    public Session()
    {
        Id = Guid.NewGuid();
        _sessionItems = [];

        PropertyChanged += (sender, args) =>
        {
            Trace.WriteLine($"Session: {args.PropertyName}");
            foreach (var g in GroupedSessionItems.CollectionOfGroupings)
            {
                g.RaisePropertyChanged("Key"); 
            }
        };
    }
}

[INotifyPropertyChanged]
public partial class Grouping : System.Linq.IGrouping<string, SessionExerciceSerie>
{
    [ObservableProperty]
    public IGrouping<string, SessionExerciceSerie> _group;
    
    public IEnumerator<SessionExerciceSerie> GetEnumerator()
    {
        return Group.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [ObservableProperty] public string _key;
    
    public Grouping(System.Linq.IGrouping<string, SessionExerciceSerie> grouping)
    {
        Group = grouping;
        Key = Group.Count() == 0 ? "" : Group.Key;
    }

    public void RaisePropertyChanged(string property)
    { 
        OnPropertyChanged(property);    
    }
}

public partial class GroupedSessionItemsType : ObservableObject
{
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(Key))] 
    [NotifyPropertyChangedFor(nameof(GroupingsCount))]
    public ObservableCollection<Grouping> _collectionOfGroupings = [];

    [ObservableProperty]
    public string _key = string.Empty;

    public static GroupedSessionItemsType Instance;

    public static GroupedSessionItemsType GetInstance()
    {
        if (Instance is not null)
            return Instance;

        Instance = new();
        return Instance;
    }
    
    public int GroupingsCount => CollectionOfGroupings?.Count ?? 0;

    public GroupedSessionItemsType()
    {
    }

    public GroupedSessionItemsType(IEnumerable<System.Linq.IGrouping<string, SessionExerciceSerie>> collection)
    {
        PropertyChanged += (sender, args) =>
        {
            var propName = "GroupingsCount";
            Trace.WriteLine($"GroupedSessionItemsType: {args.PropertyName},{sender.GetType()}");
            if (args.PropertyName == propName)
                Trace.WriteLine($"  GroupingCounts: {GroupingsCount}");
        };
        
        CollectionOfGroupings.Clear();
        collection.Select(g => new Grouping(g)).ToList().ForEach(g =>
        {
            CollectionOfGroupings.Add(g);
        });
        Key = CollectionOfGroupings.Count == 0 ? "" : CollectionOfGroupings[0].Key ?? "";
        OnPropertyChanged(nameof(GroupingsCount));
    }

    public void RaisePropertyChanged(string property)   
    {
        OnPropertyChanged(property);
    }

    public static GroupedSessionItemsType InstanceWithCollection(IEnumerable<IGrouping<string, SessionExerciceSerie>> collection)
    {
        var instance = GetInstance();
        
        instance.CollectionOfGroupings.Clear();
        collection.Select(g => new Grouping(g)).ToList().ForEach(g =>
        {
            instance.CollectionOfGroupings.Add(g);
        });
        instance.Key = Instance.CollectionOfGroupings.Count == 0 ? "" : instance.CollectionOfGroupings[0].Key ?? "";
        //instance.OnPropertyChanged(nameof(GroupingsCount));

        return instance;
    }
}
