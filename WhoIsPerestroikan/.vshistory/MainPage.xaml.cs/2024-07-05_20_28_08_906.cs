namespace WhoIsPerestroikan
{
    public partial class MainPage : ContentPage
    {
        private LocationService _locationService;
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
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
