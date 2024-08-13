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
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}