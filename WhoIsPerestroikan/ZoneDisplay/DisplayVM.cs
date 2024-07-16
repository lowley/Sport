using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using WhoIsPerestroikan;

namespace WhoIsPerestroikan.VM
{
    public partial class DisplayVM : ObservableObject
    {
        [ObservableProperty]
        public BindingList<MapPin> _customPins = [];
        public MapPin PinMoi { get; set; }
        public MapPin PinPeres { get; set; }
        public CustomMapHandler MapHandler { get; set; }

        [RelayCommand]
        public void MoveMe()
        {
            PinMoi.Location = new Location(
            (PinMoi.Location.Latitude + PinPeres.Location.Latitude) / 2,
            (PinMoi.Location.Longitude + PinPeres.Location.Longitude) / 2
            );
            MapHandler?.MovePin(PinMoi);
        }

        public void AddPinsMoiPeres()
        {
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
            CustomPins.Add(PinMoi);
            CustomPins.Add(PinPeres);
        }

        public void UpdatePinMoiWithPosition(Location newLocation)
        {
            CustomPins[0].Location.UpdateWith(newLocation);
            //OnPropertyChanged(nameof(Location));
            RaisePropertyChanged();
            MapHandler?.MovePin(CustomPins[0]);
        }

        public void RaisePropertyChanged()
        {
            OnPropertyChanged(nameof(CustomPins));
        }

        public DisplayVM()
        {
            CustomPins.ListChanged += (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));
        }
    }
}
