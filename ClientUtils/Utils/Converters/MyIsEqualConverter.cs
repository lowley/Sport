namespace ClientUtilsProject.Utils;

using System;
using System.Globalization;

public class MyIsEqualConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        Object equals = null;
        Object notequals = null;

        if (targetType == typeof(Color))
        {
            equals = Colors.Brown;
            notequals = Colors.Goldenrod;
        }

        if (targetType == typeof(double))
        {
            equals = 3;
            notequals = 1;
        }

        if (values is null || values.Count() != 2
                           || values[0] is null || values[1] is null)
            return notequals;

        var first =(int) values[0];
        var second =(int) values[1];

        if (first == second)
            return equals;
        else return notequals;

    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    
    public static object GetPropertyValue(object src, string propertyName)
    {
        if (propertyName.Contains('.'))
        {
            var splitIndex = propertyName.IndexOf('.');
            var parent = propertyName.Substring(0, splitIndex);
            var child = propertyName.Substring(splitIndex + 1);
            var obj = src?.GetType().GetProperty(parent)?.GetValue(src, null);
            return GetPropertyValue(obj, child);
        }

        var type = src?.GetType();
        var property = type?.GetProperty(propertyName);
        var value = property?.GetValue(src, null);

        return value;
    }
}