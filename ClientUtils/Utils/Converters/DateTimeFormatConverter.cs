using Microsoft.Maui.Controls;

namespace ClientUtilsProject.Utils;

using System;
using System.Globalization;

public class DateTimeFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime && parameter is string format)
        {
            return dateTime.ToString(format, culture);
        }
        
        if (value is TimeSpan timeSpan && parameter is string format2)
        {
            return timeSpan.ToString(format2);
            //return string.Format(culture, format2, timeSpan);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}