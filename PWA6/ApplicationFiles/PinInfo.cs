using CommunityToolkit.Mvvm.ComponentModel;

namespace WhoIsPerestroikan
{
    public partial class PinInfo : ObservableObject
    {
        [ObservableProperty]
        public Location _location;

        [ObservableProperty]
        public string _label;

        [ObservableProperty]
        public string _address;
    }
}
