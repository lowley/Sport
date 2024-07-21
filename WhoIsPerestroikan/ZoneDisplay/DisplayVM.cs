using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public CommunicationWithServer CommunicationWithServer { get; set; }

        private  name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }



        [RelayCommand]
        public async Task MoveMe()
        {
            var nvoPin = new MapPin
            {
                Id = Guid.NewGuid().ToString(),
                Label = "test",
                Location = new Location(
                    (PinMoi.Location.Latitude + PinPeres.Location.Latitude) / 2,
                    (PinMoi.Location.Longitude + PinPeres.Location.Longitude) / 2),
                Icon = "personpin.png",
                IconWidth = 60,
                IconHeight = 80
            };


            try
            {
                CustomPins.Add(nvoPin);
                MapHandler?.AddPin(nvoPin);
                OnPropertyChanged(nameof(CustomPins));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }

            //await CommunicationWithServer.ClearPinDTOS();

            //PinMoi.Location = new Location(
            //(PinMoi.Location.Latitude + PinPeres.Location.Latitude) / 2,
            //(PinMoi.Location.Longitude + PinPeres.Location.Longitude) / 2
            //);
            //MapHandler?.MovePin(PinMoi);
        }

        public void AddPinsMoiPeres()
        {
            var number = new Random().NextInt64(1, 1000000);
            PinMoi = new MapPin
            {
                Label = $"Utilisateur {number}",
                Location = new Location(48.58402, 7.74750),
                Icon = "personpin.png",
                IconWidth = 60,
                IconHeight = 80
            };
            CustomPins.Add(PinMoi);

            PinPeres = new MapPin
            {
                Label = "Perestroïka",
                Location = new Location(48.58432, 7.73750),
                Icon = "coffeepin.png",
                IconWidth = 80,
                IconHeight = 80
            };

            if (CustomPins.All(pin => !pin.Label.Equals(PinPeres.Label)))
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
        public void ClearOtherPins()
        {
            var ToDelete = CustomPins
                .Where(pin => pin.Label != PinPeres.Label && pin.Label != PinMoi.Label);

            ToDelete.ForEach(pin =>
            {
                CustomPins.Remove(pin);
                MapHandler?.RemovePin(pin);
            });

            //AddPinsMoiPeres();
            //Task.Run(async () => await CommunicationWithServer.AddMapPin(PinMoi));
            OnPropertyChanged(nameof(CustomPins));
        }
        public void AddOtherPins(List<MapPinDTO> others)
        {
            others.ForEach(pinDTO =>
            {
                var pin = new MapPin
                {
                    Id = pinDTO.Id,
                    Label = pinDTO.Label,
                    Location = new Location(pinDTO.Latitude, pinDTO.Longitude, pinDTO.Altitude),
                    Icon = "personpin.png",
                    IconWidth = 60,
                    IconHeight = 80
                };

                CustomPins.Add(pin);
            });

            //todo laisser le handler ajouter ce qui n'existe pas
            //observableCollection?
            MapHandler?.AddPin(CustomPins.Last());
            OnPropertyChanged(nameof(CustomPins));
        }

        public DisplayVM(CommunicationWithServer com)
        {
            CustomPins.ListChanged += (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));
            CommunicationWithServer = com;
        }
    }
}
