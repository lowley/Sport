using System.Globalization;
using ClientUtilsProject.DataClasses;

namespace ClientUtilsProject.Utils;

public class IsNotNullAndHasNameConverter : IMultiValueConverter
{
    public object? Convert(object?[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is null)
            return false;

        var numberOfNotNullsToTest = parameter is null ? 1 : int.Parse((string)parameter);

        var result = false;
        
        if (values[0] is Exercise exo)
            result = exo.ExerciseName != string.Empty;

        if (numberOfNotNullsToTest == 2)
            result = result && values[1] is not null;
        
        return result;
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        if (!(value is bool b) || targetTypes.Any(t => !t.IsAssignableFrom(typeof(bool))))
        {
            // Return null to indicate conversion back is not possible
            return null;
        }

        if (b)
        {
            return targetTypes.Select(t => (object)true).ToArray();
        }
        else
        {
            // Can't convert back from false because of ambiguity
            return null;
        }
    }
}