using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace WhoIsPerestroikan.VM
{
    public partial class DisplayVM : ObservableObject
    {
        [ObservableProperty]
        public Location _location = new Location();
        [ObservableProperty]
        public ObservableCollection<MapPin> _customPins = [];
        [ObservableProperty]
        public MapPin _pinMoi;
        [ObservableProperty]
        public MapPin _pinPeres;
        public CancellationTokenSource Cts { get; set; }
        private string LocationStatus { get; set; }
        [ObservableProperty]
        public bool _isPopupOpen;
        [ObservableProperty]
        public DataTemplate _popupTemplate;
        [ObservableProperty]
        public MapEx _map;




        public async Task InitializeMap()
        {
            try
            {
                Location.UpdateWith(await Geolocation.GetLastKnownLocationAsync());
                OnPropertyChanged(nameof(Location));

                //if (GoogleMap.CustomPins.All(pi => pi.Id != PinMoi.Id) && Location != null)
                //    GoogleMap.CustomPins.Add(PinMoi);

                // Start the location update loop
                StartLocationUpdates();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ShowPopupMessage(ex.Message);
            }
        }

        public void StartLocationUpdates()
        {
            Cts = new CancellationTokenSource();
            UpdateLocationAsync(Cts.Token);
        }

        private void ShowPopupMessage(string message)
        {
            DataTemplate templateView = new DataTemplate(() =>
            {
                Label popupContent = new Label();
                popupContent.Text = $"{message}";
                popupContent.HorizontalTextAlignment = TextAlignment.Center;
                return popupContent;
            });

            // Adding ContentTemplate of the SfPopup
            PopupTemplate = templateView;
            IsPopupOpen = true;
        }

        private async Task UpdateLocationAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var newLocation = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Best,
                        Timeout = TimeSpan.FromSeconds(30)
                    });

                    Location.UpdateWith(newLocation);

                    AddPinMoiInfoIfNeededAndPossible(Location);
                    UpdatePin(PinMoi, Location);
                }
                catch (Exception ex)
                {
                    LocationStatus = $"An error occurred: {ex.Message}";
                    Console.WriteLine(ex.ToString());
                    ShowPopupMessage(ex.Message);
                }

                // Attendez 3 secondes avant de demander une nouvelle localisation
                await Task.Delay(1000, token);
            }
        }

        private void AddPinMoiInfoIfNeededAndPossible(Location location)
        {
            if (Map.CustomPins.All(pin => pin.Label != PinMoi.Label) && location != null)
                Map.CustomPins.Add(PinMoi);
        }

        void UpdatePin(MapPin pin, Location location)
        {
            if (pin != null)
                pin.Location = location;
            //OnPropertyChanged(nameof(GoogleMap));
        }

        public DisplayVM()
        {
            
        }
    }
}
