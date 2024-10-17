using ClientUtilsProject.DataClasses;
using ClientUtilsProject.ViewModels;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using SportProject;
using SportProject.Resources.Styles;
using Syncfusion.Maui.DataSource.Extensions;

namespace Sport
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWGNCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXZcdXVUR2ldUk1wVkY=");
            ExercisesVM._exercices = [];
            
            InitializeComponent();
            
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
            
            MainPage = new AppShell();
        }
        
        private void SetDarkTheme()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new DarkTheme());
                mergedDictionaries.Add(new Styles());
                AddRemainder(mergedDictionaries);
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
                AddRemainder(mergedDictionaries);
            }
        }

        public void AddRemainder(ICollection<ResourceDictionary> dicos)
        {
            dicos.Add(new RemainderThemeContent());
            
            if (dicos.Any(rd => rd.ContainsKey("SixtyColor"))
                && dicos.Any(rd => rd.ContainsKey("TextColor")))
            {
                var sixtyColor = (Color)dicos.First(rd => rd.ContainsKey("SixtyColor"))
                    .Values.First();
                var textColor = (Color)dicos.First(rd => rd.ContainsKey("TextColor"))
                    .Values.First();
                
                var pagesStyle = new Style(typeof(ContentPage));
                // pagesStyle.Behaviors.Add(new CommunityToolkit.Maui.Behaviors.StatusBarBehavior()
                // {
                //     StatusBarColor = sixtyColor,
                //     //StatusBarStyle = StatusBarStyle.DarkContent
                // });

                var shellStyle = new Style(typeof(Shell));
                // shellStyle.Behaviors.Add(new CommunityToolkit.Maui.Behaviors.StatusBarBehavior
                // {
                //     StatusBarColor = sixtyColor,
                // });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.TabBarBackgroundColorProperty,
                    Value = sixtyColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.TabBarTitleColorProperty,
                    Value = textColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.TabBarForegroundColorProperty,
                    Value = textColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.TabBarUnselectedColorProperty,
                    Value = textColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.BackgroundColorProperty,
                    Value = sixtyColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.ForegroundColorProperty,
                    Value = textColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.UnselectedColorProperty,
                    Value = textColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.DisabledColorProperty,
                    Value = textColor
                });
                shellStyle.Setters.Add(new Setter()
                {
                    Property = Shell.TitleColorProperty,
                    Value = textColor
                });
                dicos.Add(new (){pagesStyle, shellStyle});
                
                
                //Shell.SetTabBarBackgroundColor(this, sixtyColor);
            }
        }
    }
}
