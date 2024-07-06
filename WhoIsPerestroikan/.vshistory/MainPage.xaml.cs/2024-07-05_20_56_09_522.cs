namespace WhoIsPerestroikan
{
    public partial class MainPage : ContentPage
    {
        private LocationService _locationService;
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        private string LocationStatus { get; set; }
        private CancellationTokenSource _cts;

        public Location Location { get; set; }

        public void StartLocationUpdates()
        {
            _cts = new CancellationTokenSource();
            UpdateLocationAsync(_cts.Token);
        }

        public void StopLocationUpdates()
        {
            _cts?.Cancel();
        }

        private async Task UpdateLocationAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });

                    if (location != null)
                    {
                        LocationStatus = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
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

            _locationService = new LocationService();
            StartLocationService();
        }

        private async void StartLocationService()
        {
            // Demandez la permission de localisation à l'utilisateur
            var status = await Permissions.RequestAsync<Permissions.LocationAlways>();

            if (status == PermissionStatus.Granted)
            {
                await _locationService.StartLocationUpdatesAsync();
            }
            else
            {
                await DisplayAlert("Permission Denied", "Unable to get location permission.", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _locationService.StopLocationUpdates();
        }
    }
}
