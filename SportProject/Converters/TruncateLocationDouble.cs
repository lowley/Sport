using System.Globalization;

namespace Sport.Converters
{
    public class TruncateLocationDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            var valueToTruncate = (double)value;
            var digits = int.Parse((string)parameter);
            return valueToTruncate.ToString($"F{digits}", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
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

            return src?.GetType().GetProperty(propertyName)?.GetValue(src, null);
        }
    }
}
