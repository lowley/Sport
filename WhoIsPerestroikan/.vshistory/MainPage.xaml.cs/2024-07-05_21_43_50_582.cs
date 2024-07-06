using Mapsui;
using Mapsui.Projections;

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
                        var center = new MPoint(SphericalMercator.FromLonLat(Location.Longitude, Location.Latitude));
                        mapView.Map.Navigator.CenterOnAndZoomTo(center, 1000); // Adjust the zoom level to 100m accuracy
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
                await Task.Delay(1000, token);
                OnPropertyChanged(nameof(Location));
            }
        }

        public MainPage()
        {
            InitializeComponent();

            InitializeMap();
            LocationService = new LocationService();
            StartLocationService();
        }

        private async Task InitializeMap()
        {
            mapView.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location != null)
            {
                var center = new MPoint(SphericalMercator.FromLonLat(Location.Longitude, Location.Latitude));
                mapView.Map.Navigator.CenterOnAndZoomTo(center, 1000); // Adjust the zoom level to 100m accuracy
            }

            // Start the location update loop
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
