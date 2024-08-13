using Sport.Pages;
using SportProject.Pages;

namespace Sport
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("sessions/session", typeof(SessionPage));
            Routing.RegisterRoute("sessions/sessions", typeof(SessionsPage));
            Routing.RegisterRoute("exercises/exercise", typeof(ExercisePage));
            Routing.RegisterRoute("exercises/exercises", typeof(ExercisesPage));

            InitializeComponent();
        }
    }
}
