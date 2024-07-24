using Serilog.Core;
using System.Diagnostics;
using WhoIsPerestroikan.VM;
using WIPClient.Utils;
using WIPClientVM;

namespace WhoIsPerestroikan;

public partial class DisplayPage : ContentPage
{
    public DisplayVM VM { get; set; }
    private Geolocalisation LocationService { get; set; }
    private Logger Logger { get; set; }

    public DisplayPage(DisplayVM vM, Geolocalisation locationService, Logger logger)
    {
        VM = vM;
        LocationService = locationService;
        Logger = logger;
    }
}

