using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ClientUtilsProject.Utils;

public class FirstNotEmptyConverter : IMultiValueConverter
{
    public object? Convert(object?[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || !targetType.IsAssignableFrom(typeof(string)))
            return false;

        foreach (var value in values)
        {
            if (!(value is string text))
                return string.Empty;
            
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
        }
        return string.Empty;
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}