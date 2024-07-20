using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows.Input;
using WhoIsPerestroikan;

namespace WhoIsPerestroikan
{
    [Serializable]
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
}

public class MapPinDTO
{
    public string Id { get; set; }
    public string Label { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
}

public static class MapPinTools
{
    public static MapPinDTO ToDTO(this MapPin mapPin)
    {
        return new MapPinDTO
        {
            Id = mapPin.Id,
            Label = mapPin.Label,
            Latitude = mapPin.Latitude,
            Longitude = mapPin.Longitude,
            Altitude = mapPin.Altitude,
        };
    }
}