using Serilog.Core;
using System.Diagnostics;
using WIPClient.Utils;
using WIPClientVM;

namespace WIPClient.DisplayPage;

public partial class DisplayPage : ContentPage
{
    public DisplayVM VM { get; set; }
    private Geolocalisation LocationService { get; set; }
    private Logger Logger { get; set; }

    public DisplayPage(DisplayVM vm, Geolocalisation locationService, Logger logger)
    {
        VM = vm;
        BindingContext = VM;
        LocationService = locationService;
        Logger = logger;
    }
}

