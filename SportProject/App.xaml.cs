using ClientUtilsProject.DataClasses;
using ClientUtilsProject.ViewModels;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace Sport
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQzMzg5NUAzMjM2MmUzMDJlMzBCK0ljNklpMjRCcHo1aTBtem5YaGFMUXZWWjFySU44ZWQ4VWovUHlCeDVvPQ==");
            ExercisesVM._exercices = [];
            SessionsVM._sessions = [];
            

            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
