using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WhoIsPerestroikan.VM
{
    public partial class DisplayVM : ObservableObject
    {
        [ObservableProperty]
        public BindingList<MapPin> _customPins = [];
        public MapPin PinMoi { get; set; }
        public MapPin PinPeres { get; set; }

        public CustomMapHandler MapHandler { get; set; }

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

        public DisplayVM()
        {
            CustomPins.ListChanged += (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));


        }
    }
}
