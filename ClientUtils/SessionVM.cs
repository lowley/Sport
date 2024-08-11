using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace Sport.VM;

public partial class SessionVM : ObservableObject
{
    [ObservableProperty]
    public DateTime _sessionDate = DateTime.Now;

    [ObservableProperty]
    public TimeSpan _sessionTime = DateTime.Now.TimeOfDay;

    [RelayCommand]
    public async Task CloseSession()
    {
        
        
        
    }


    public SessionVM()
    {
        
    }
}
