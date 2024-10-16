using ClientUtilsProject.DataClasses;
using ClientUtilsProject.ViewModels;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using SportProject.Resources.Styles;

namespace Sport
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWGNCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXZcdXVUR2ldUk1wVkY=");
            ExercisesVM._exercices = [];
            

            InitializeComponent();
            MainPage = new AppShell();
            
            if (RequestedTheme == AppTheme.Dark)
            {
                SetDarkTheme();
            }
            else
            {
                SetLightTheme();
            }

            RequestedThemeChanged += (s, a) =>
            {
                if (a.RequestedTheme == AppTheme.Dark)
                {
                    SetDarkTheme();
                }
                else
                {
                    SetLightTheme();
                }
            };
        }
        
        private void SetDarkTheme()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new DarkTheme());
                mergedDictionaries.Add(new Styles());
                Addremainder(mergedDictionaries);
            }
        }

        private void SetLightTheme()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new LightTheme());
                mergedDictionaries.Add(new Styles());
                Addremainder(mergedDictionaries);
            }
        }

        public void Addremainder(ICollection<ResourceDictionary> dicos)
        {
            dicos.Add(new RemainderThemeContent());
        }
    }
}
