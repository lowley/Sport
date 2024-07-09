using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using WhoIsPerestroikan.VM;
using MapSpan = Microsoft.Maui.Maps.MapSpan;

namespace WhoIsPerestroikan;

public partial class DisplayPage : ContentPage
{
    public DisplayVM ViewModel { get; set; }

    private LocationService LocationService;
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    private string LocationStatus { get; set; }
    private CancellationTokenSource Cts { get; set; }
    public Location Location { get; set; } = new Location();
    public Location PerestroikaPosition { get; set; } = new Location(48.58432d, 7.73750d);
    //public BitmapDescriptor MoiIcon { get; set; }
    public MapPin PinMoi { get; set; }
    public MapPin PinPeres { get; set; }

    public void StopLocationUpdates()
    {
        Cts?.Cancel();
    }

    private async Task InitializeMap()
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
        if (GoogleMap.CustomPins.All(pin => pin.Label != PinMoi.Label) && location != null)
            GoogleMap.CustomPins.Add(PinMoi);
    }

    void UpdatePin(MapPin pin, Location location)
    {
        if (pin != null)
            pin.Location = location;
        OnPropertyChanged(nameof(GoogleMap));
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
        popup.ContentTemplate = templateView;
        popup.Show();
    }

    private async void StartLocationService()
    {
        LocationService = new LocationService();
        // Demandez la permission de localisation à l'utilisateur
        var status = await Permissions.RequestAsync<Permissions.LocationAlways>();

        if (status == PermissionStatus.Granted)
            await LocationService.StartLocationUpdatesAsync();
        else
            await DisplayAlert("Permission Denied", "Unable to get location permission.", "OK");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        LocationService.StopLocationUpdates();
    }

    public async Task<Stream> ReadTextFile(string file)
    => await FileSystem.Current.OpenAppPackageFileAsync(file);

    public DisplayPage(DisplayVM vm)
    {
        InitializeComponent();
        ViewModel = vm;
        BindingContext = ViewModel;

        StartLocationService();

        PinPeres = new MapPin
        {
            Label = "Perestroïka",
            Location = new Location(48.58432, 7.73750),
            Icon = "coffeepin.png",
            IconWidth = 80,
            IconHeight = 80
        };

        PinMoi = new MapPin
        {
            Label = "Moi",
            Location = new Location(48.58402, 7.74750),
            Icon = "personpin.png",
            IconWidth = 60,
            IconHeight = 80
        };

        GoogleMap.CustomPins.Add(PinMoi);
        GoogleMap.CustomPins.Add(PinPeres);

        InitializeMap();
    }

    private void MoveMe_Clicked(object sender, EventArgs e)
    {
        var handler = GoogleMap.Handler;
        PinMoi.Location = new Location(
            (PinMoi.Location.Latitude + PinPeres.Location.Latitude) / 2,
            (PinMoi.Location.Longitude + PinPeres.Location.Longitude) / 2
            );
        (handler as CustomMapHandler).MovePin(PinMoi);
    }
}

public partial class PinInfo : ObservableObject
{
    [ObservableProperty]
    public Location _location;

    [ObservableProperty]
    public string _label;

    [ObservableProperty]
    public string _address;
}

public static class Tools
{
    public static void UpdateWith(this Location oldLocation, Location newLocation)
    {
        oldLocation.Latitude = newLocation.Latitude;
        oldLocation.Longitude = newLocation.Longitude;
        oldLocation.Altitude = newLocation.Altitude;
        oldLocation.Accuracy = newLocation.Accuracy;
    }
}

public partial class MapPin : ObservableObject
{
    [ObservableProperty]
    public string _id;

    [ObservableProperty]
    public Location _location;

    [ObservableProperty]
    public string _label;

    [ObservableProperty]
    public string _icon;

    [ObservableProperty]
    public int _iconWidth;

    [ObservableProperty]
    public int _iconHeight;

    public ICommand ClickedCommand { get; set; } = new Command(() => { });
    public MapPin()
    {
    }

    public MapPin(Action<MapPin> clicked)
    {
        ClickedCommand = new Command(() => clicked(this));
    }
}

public class MapEx : Microsoft.Maui.Controls.Maps.Map
{
    public MapEx(MapSpan region) : base(region) 
    {
        CustomPins = [];
    }

    public List<MapPin> CustomPins
    {
        get { return (List<MapPin>)GetValue(CustomPinsProperty); }
        set { SetValue(CustomPinsProperty, value); }
    }

    public static readonly BindableProperty CustomPinsProperty = BindableProperty.Create(nameof(CustomPins), typeof(List<MapPin>), typeof(MapEx), null);
}

