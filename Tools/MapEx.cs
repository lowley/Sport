using System.Collections.ObjectModel;
using System.ComponentModel;
using MapSpan = Microsoft.Maui.Maps.MapSpan;

namespace WhoIsPerestroikan
{
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
}
