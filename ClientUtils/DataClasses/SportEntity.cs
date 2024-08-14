using System.ComponentModel;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public class SportEntity : ObservableObject
{
    private readonly Dictionary<string, INotifyPropertyChanged> _propertySubscriptions = new();

    public void ManageChangedEventForProperties()
    {
        var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes(typeof(ObservablePropertyAttribute), true);
            if (attributes.Any())
            {
                var propertyType = property.PropertyType;
                if (typeof(INotifyPropertyChanged).IsAssignableFrom(propertyType))
                {
                    if (property.GetValue(this) is INotifyPropertyChanged currentValue)
                    {
                        SubscribePropertyChanged(property.Name, currentValue);
                    }
                }
            }
        }
    }

    private void SubscribePropertyChanged(string propertyName, INotifyPropertyChanged currentValue)
    {
        if (_propertySubscriptions.ContainsKey(propertyName))
        {
            var oldValue = _propertySubscriptions[propertyName];
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= CreateDynamicHandler(propertyName);
            }
        }

        if (currentValue != null)
        {
            currentValue.PropertyChanged += CreateDynamicHandler(propertyName);
            _propertySubscriptions[propertyName] = currentValue;
        }
        else
        {
            _propertySubscriptions.Remove(propertyName);
        }
    }

    private PropertyChangedEventHandler CreateDynamicHandler(string propertyName)
    {
        return (sender, e) => OnPropertyChanged($"{propertyName}");
    }

    public SportEntity()
    {
        ManageChangedEventForProperties();
    }
}