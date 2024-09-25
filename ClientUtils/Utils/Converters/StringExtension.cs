namespace ClientUtilsProject.Utils.Converters;

public class StringExtension : IMarkupExtension
{
    public string Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}