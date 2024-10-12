﻿namespace ClientUtilsProject.Utils;

using System;
using System.Globalization;

public class MyIsNotNullConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value != null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
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