using Sport.Pages;

namespace Sport
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("sessions/session", typeof(SessionPage));
            Routing.RegisterRoute("exercises/exercise", typeof(ExercisePage));
            Routing.RegisterRoute("exercises/exercises", typeof(ExercisesPage));

            InitializeComponent();
        }
    }
}
