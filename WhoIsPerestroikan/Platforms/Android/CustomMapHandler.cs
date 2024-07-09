using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Android.Graphics;
using Microsoft.Maui.Maps.Handlers;
using WhoIsPerestroikan;
using System.Diagnostics;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

public class CustomMapHandler : MapHandler
{
    private const int _iconSize = 60;

    private readonly Dictionary<string, BitmapDescriptor> _iconMap = [];

    public static new IPropertyMapper<MapEx, CustomMapHandler> Mapper = new PropertyMapper<MapEx, CustomMapHandler>(MapHandler.Mapper)
    {
        [nameof(MapEx.CustomPins)] = MapPins
    };

    public Dictionary<string, (Marker Marker, MapPin Pin)> MarkerMap { get; } = [];

    public CustomMapHandler()
        : base(Mapper)
    {
    }

    protected override void ConnectHandler(MapView platformView)
    {
        base.ConnectHandler(platformView);
        var mapReady = new MapCallbackHandler(this);

        PlatformView.GetMapAsync(mapReady);
    }

    private static new void MapPins(IMapHandler handler, Microsoft.Maui.Maps.IMap map)
    {
        if (handler.Map is null || handler.MauiContext is null)
        {
            return;
        }

        if (handler is CustomMapHandler mapHandler)
        {
            foreach (var marker in mapHandler.MarkerMap)
            {
                marker.Value.Marker.Remove();
            }

            mapHandler.MarkerMap.Clear();

            mapHandler.AddPins();
        }
    }

    private void AddPins()
    {
        if (VirtualView is MapEx mapEx && mapEx.CustomPins != null)
        {
            foreach (var pin in mapEx.CustomPins)
            {
                var markerOption = new MarkerOptions();
                markerOption.SetTitle(pin.Label);
                markerOption.SetIcon(GetIcon(pin));
                markerOption.SetPosition(new LatLng(pin.Location.Latitude, pin.Location.Longitude));
                var marker = Map.AddMarker(markerOption);

                MarkerMap.Add(marker.Id, (marker, pin));
            }
        }
    }

    private BitmapDescriptor GetIcon(MapPin pin)
    {
        if (_iconMap.TryGetValue(pin.Icon, out BitmapDescriptor? value))
        {
            return value;
        }

        //var service = new FileImageSourceService();
        //var bitmap = service.LoadDrawableAsync("cat.png", ...).Result;

        var handler = new FileImageSourceHandler();
        var img = ImageSource.FromFile(pin.Icon);
        Bitmap bitmap = handler.LoadImageAsync(img, Android.App.Application.Context).Result;

        //var drawable = Context.Resources.GetIdentifier(icon, "drawable", Context.PackageName);
        //var bitmap = BitmapFactory.DecodeResource(Context.Resources, drawable);
        var scaled = Bitmap.CreateScaledBitmap(bitmap, pin.IconWidth, pin.IconHeight, false);
        bitmap.Recycle();
        var descriptor = BitmapDescriptorFactory.FromBitmap(scaled);

        _iconMap[pin.Icon] = descriptor;

        return descriptor;
    }

    public void MovePin(MapPin newPin)
    {
        if (VirtualView is MapEx mapEx && mapEx.CustomPins != null)
        {
            var pin = mapEx.CustomPins.FirstOrDefault(p => p.Id == newPin.Id);
            if (pin == null)
                return;

            var marker = MarkerMap.FirstOrDefault(mm => mm.Value.Pin.Id == newPin.Id);
            marker.Value.Marker.Position = new LatLng(newPin.Location.Latitude,newPin.Location.Longitude);
            marker.Value.Pin.Location = new Location(newPin.Location.Latitude, newPin.Location.Longitude);
        }
    }

    public void MarkerClick(object sender, GoogleMap.MarkerClickEventArgs args)
    {
        if (MarkerMap.TryGetValue(args.Marker.Id, out (Marker Marker, MapPin Pin) value))
        {
            value.Pin.ClickedCommand?.Execute(null);
        }
    }
}

public class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
{
    private readonly CustomMapHandler mapHandler;

    public MapCallbackHandler(CustomMapHandler mapHandler)
    {
        this.mapHandler = mapHandler;
    }

    public void OnMapReady(GoogleMap googleMap)
    {
        mapHandler.UpdateValue(nameof(MapEx.CustomPins));
        googleMap.MarkerClick += mapHandler.MarkerClick;
    }
}