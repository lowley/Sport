using Serilog.Core;
using System.Diagnostics;
using WIPClientVM;

namespace WIPClient.HomePage;

public partial class HomePage : ContentPage
{
    public HomeVM VM { get; set; }
    private Logger Logger { get; set; }

    public HomePage(HomeVM vm, Logger logger)
    {
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }
}

