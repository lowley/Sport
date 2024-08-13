using ClientUtilsProject.Utils;

namespace Sport.Converters;

public class SportNavigation: ISportNavigation
{
    public async Task NavigateTo(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("..");

    }
}