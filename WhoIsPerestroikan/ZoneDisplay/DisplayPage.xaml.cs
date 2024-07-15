using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows.Input;
using WhoIsPerestroikan.VM;
using MapSpan = Microsoft.Maui.Maps.MapSpan;

namespace WhoIsPerestroikan;

public partial class DisplayPage : ContentPage
{
    public DisplayVM VM { get; set; }

    private LocationService LocationService;
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    private string LocationStatus { get; set; }
    private CancellationTokenSource Cts { get; set; }
    public Location Location { get; set; } = new Location();

    public void StartLocationUpdates()
    {
        Cts = new CancellationTokenSource();
        UpdateLocationAsync(Cts.Token);
    }

    public void StopLocationUpdates()
    {
        Cts?.Cancel();
    }

    private async Task InitializeMap()
    {
        try
        {
            VM.UpdatePinMoiWithPosition(await Geolocation.GetLastKnownLocationAsync());

            // Start the location update loop
            StartLocationUpdates();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            ShowPopupMessage(ex.Message);
        }
    }

    private async Task UpdateLocationAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (VM.MapHandler == null && GoogleMap.Handler != null)
                VM.MapHandler = GoogleMap.Handler as CustomMapHandler;

            try
            {
                VM.UpdatePinMoiWithPosition(await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(30)
                }));
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        LocationService.StopLocationUpdates();
    }

    public DisplayPage(DisplayVM vm, LocationService locationService)
    {
        LocationService = locationService;
        InitializeComponent();

        if (!LocationService.RequestLocationPermission().Result)
            ShowPopupMessage("Permission requise pour obtenir votre position");
        VM = vm;

        GoogleMap.SetBinding(MapEx.CustomPinsProperty, new Binding(source: VM, path: "CustomPins", mode: BindingMode.TwoWay));
        BindingContext = VM;

        VM.AddPinsMoiPeres();
        InitializeMap();
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

public partial class MapPin : ObservableObject
{
    [ObservableProperty]
    public string _id;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Latitude))]
    [NotifyPropertyChangedFor(nameof(Longitude))]
    [NotifyPropertyChangedFor(nameof(Altitude))]
    public Location _location;

    [ObservableProperty]
    public string _label;

    [ObservableProperty]
    public string _icon;

    [ObservableProperty]
    public int _iconWidth;

    [ObservableProperty]
    public int _iconHeight;

    public double Latitude => Location?.Latitude ?? 48.58432;
    public double Longitude => Location?.Longitude ?? 7.73750;
    public double Altitude => Location?.Altitude ?? 226;

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
        CustomPins.ListChanged +=
            (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));
    }

    public BindingList<MapPin> CustomPins
    {
        get { return (BindingList<MapPin>)GetValue(CustomPinsProperty); }
        set { SetValue(CustomPinsProperty, value); }
    }

    public static readonly BindableProperty CustomPinsProperty = BindableProperty.Create(nameof(CustomPins), typeof(BindingList<MapPin>), typeof(MapEx), null);
}

