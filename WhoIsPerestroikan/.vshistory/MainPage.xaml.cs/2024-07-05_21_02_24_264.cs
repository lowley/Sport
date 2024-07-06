namespace WhoIsPerestroikan
{
    public partial class MainPage : ContentPage
    {
        private LocationService LocationService;
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        private string LocationStatus { get; set; }
        private CancellationTokenSource Cts { get; set; }

        public Location Location { get; set; }

        public void StartLocationUpdates()
        {
            Cts = new CancellationTokenSource();
            UpdateLocationAsync(Cts.Token);
        }

        public void StopLocationUpdates()
        {
            Cts?.Cancel();
        }

        private async Task UpdateLocationAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    Location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });

                    if (Location != null)
                    {
                        LocationStatus = $"Latitude: {Location.Latitude}, Longitude: {Location.Longitude}";
                        OnPropertyChanged(nameof(Location));
                    }
                    else
                    {
                        LocationStatus = "Location not found.";
                    }
                }
                catch (Exception ex)
                {
                    LocationStatus = $"An error occurred: {ex.Message}";
                }

                // Attendez 3 secondes avant de demander une nouvelle localisation
                await Task.Delay(3000, token);
            }
        }

        public MainPage()
        {
            InitializeComponent();

            LocationService = new LocationService();
            StartLocationService();
            StartLocationUpdates();
        }

        private async void StartLocationService()
        {
            // Demandez la permission de localisation à l'utilisateur
            var status = await Permissions.RequestAsync<Permissions.LocationAlways>();

            if (status == PermissionStatus.Granted)
            {
                await LocationService.StartLocationUpdatesAsync();
            }
            else
            {
                await DisplayAlert("Permission Denied", "Unable to get location permission.", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            LocationService.StopLocationUpdates();
        }
    }
}
