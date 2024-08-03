using Sport.Pages;

namespace Sport
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("sessions/session", typeof(SessionPage));

            InitializeComponent();
        }
    }
}
