using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace WhoIsPerestroikan
{
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
