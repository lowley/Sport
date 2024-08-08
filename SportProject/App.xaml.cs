﻿using ClientUtilsProject.DataClasses;
using Sport.VM;
using System.Collections.ObjectModel;

namespace Sport
{
    public partial class App : Application
    {
        public App()
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM2OTY0NEAzMjM2MmUzMDJlMzBsMndqL0JCMmhrNTlxVFI2MFgxT3F1QXpyYy9GVGc1d0VPclU2TWdsWk1BPQ==");
            ExercisesVM._exercices = [];

            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
