using System.Threading.Tasks;

namespace ClientUtilsProject.Utils;

public interface ISportNavigation
{
    Task NavigateTo(string route);
    Task NavigateBack();
}