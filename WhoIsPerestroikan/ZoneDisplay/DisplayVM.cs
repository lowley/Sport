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
        [NotifyPropertyChangedFor(nameof(Moi))]
        public BindingList<MapPin> _customPins = [];

        public MapPin Moi => CustomPins.FirstOrDefault(pin => pin.Label.Equals("Moi"));


        public CustomMapHandler MapHandler { get; set; }

        public DisplayVM()
        {
            CustomPins.ListChanged += (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));


        }
    }
}
