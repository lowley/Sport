using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Sport.VM
{
    public partial class HomeVM : ObservableObject
    {
        [ObservableProperty]
        public int _counter;

        [RelayCommand]
        public void AddOne()
        {
            Counter++;
        }

        public HomeVM()
        {
            //CustomPins.ListChanged += (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));
            
        }
    }
}
