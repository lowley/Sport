namespace ClientUtilsProject.Utils;

using LanguageExt;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class OptionStringConverter : ValueConverter<Option<string>, string>
{
    public OptionStringConverter() : base(
        option => option.Match(some => some, () => null), // Conversion Option<string> -> string
        str => string.IsNullOrEmpty(str) ? Option<string>.None : Option<string>.Some(str) // Conversion string -> Option<string>
    )
    {
    }
}
