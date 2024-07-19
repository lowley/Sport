
using Serilog.Core;
using System.Diagnostics;
using WhoIsPerestroikan.VM;

namespace WhoIsPerestroikan;

public partial class DisplayPage : ContentPage
{
    public DisplayVM VM { get; set; }
    private LocationService LocationService { get; set; }
    private CommunicationWithServer CommunicationWithServer { get; set; }
    private Logger Logger  { get; set; }
    public void StartLocationUpdates()
    {
        LocationService.CreateNewCancellationTokenSource();
        UpdateLocationAsync();
    }

    private async Task InitializeMap()
    {
        try
        {
            VM.UpdatePinMoiWithPosition(await LocationService.GetLastKnownLocationAsync());

            // Start the location update loop
            StartLocationUpdates();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            ShowPopupMessage(ex.Message);
        }
    }

    private async Task UpdateLocationAsync()
    {
        while (!LocationService.Cts.IsCancellationRequested)
        {
            if (VM.MapHandler == null && GoogleMap.Handler != null)
                VM.MapHandler = GoogleMap.Handler as CustomMapHandler;

            try
            {
                VM.UpdatePinMoiWithPosition(await LocationService.GetLocationAsync());
            }
            catch (Exception ex)
            {
                LocationService.LocationStatus = $"An error occurred: {ex.Message}";
                Console.WriteLine(ex.ToString());
                ShowPopupMessage(ex.Message);
            }

            // Attendez 3 secondes avant de demander une nouvelle localisation
            await Task.Delay(1000, LocationService.Cts.Token);
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

    public DisplayPage(
        DisplayVM vm,
        LocationService locationService,
        CommunicationWithServer com,
        Logger log)
    {
        LocationService = locationService;
        CommunicationWithServer = com;
        Logger = log;
        InitializeComponent();

        if (!LocationService.RequestLocationPermission().Result)
            ShowPopupMessage("Permission requise pour obtenir votre position");
        VM = vm;

        GoogleMap.SetBinding(MapEx.CustomPinsProperty, new Binding(source: VM, path: "CustomPins", mode: BindingMode.TwoWay));
        BindingContext = VM;

        VM.AddPinsMoiPeres();
        InitializeMap();

#pragma warning disable CS4014
        CommunicationWithServer.InitializeSignalR(
        onReceiveOneMapPin: pin =>
        {
            Trace.WriteLine("nouveau message entrant: {pin.Label}");

            if (VM.CustomPins.All(p => p.Id != pin.Id))
            {
                VM.CustomPins.Add(pin);
                ShowPopupMessage($"nouveau pointeur: {pin.Label}");
            }
        },
        onReceiveAllMapPins: pins =>
        {
            Trace.WriteLine($"nouveau message entrant: {pins.Count} mapPin(s)");

            pins.ForEach(pin =>
            {
                if (VM.CustomPins.All(p => p.Label != pin.Label))
                {
                    VM.CustomPins.Add(pin);
                }
            });
        },
        onTestRetour: message =>
        {
            Trace.WriteLine(message);
        });
#pragma warning restore CS4014

        Task.Run(async () => await CommunicationWithServer.SendTest("haha"));

        try
        {
            Task.Run(async () => await CommunicationWithServer.GetMapPins());
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message.ToString());
        }

        Task.Run(async () => await CommunicationWithServer.AddMapPin(VM.PinMoi));
    }
}

