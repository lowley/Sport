namespace WhoIsPerestroikan
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM2OTY0NEAzMjM2MmUzMDJlMzBsMndqL0JCMmhrNTlxVFI2MFgxT3F1QXpyYy9GVGc1d0VPclU2TWdsWk1BPQ==");

            CopyFileToAppDataDirectory("gis_osm_buildings_a_free_1.shp");
            CopyFileToAppDataDirectory("gis_osm_buildings_a_free_1.dbf");

            //Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
            InitializeComponent();

            MainPage = new AppShell();
        }

        public void CopyFile(string filename)
        {
            var assembly = typeof(App).Assembly;
            using var stream = assembly.GetManifestResourceStream($"WhoIsPerestroikan.Resources.Raw.filename");
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(FileSystem.AppDataDirectory, filename);
            using var fileStream = File.Create(filePath);
            stream.CopyTo(fileStream);
        }

        public async Task CopyFileToAppDataDirectory(string filename)
        {
            // Open the source file
            using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);

            // Create an output filename
            string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, filename);

            // Copy the file to the AppDataDirectory
            using FileStream outputStream = File.Create(targetFile);
            await inputStream.CopyToAsync(outputStream);
        }
    }
}
