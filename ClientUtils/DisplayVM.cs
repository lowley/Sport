using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace WIPClientVM
{
    public partial class DisplayVM : ObservableObject
    {
        [ObservableProperty]
        public int _counter;

        [RelayCommand]
        public void AddOne()
        {
            Counter++;
        }

        public DisplayVM()
        {
            //CustomPins.ListChanged += (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));
            
        }
    }
}
