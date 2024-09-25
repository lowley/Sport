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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWGNCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXZcdXVUR2ldUk1wVkY=");
            ExercisesVM._exercices = [];
            SessionsVM._sessions = [];
            

            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
