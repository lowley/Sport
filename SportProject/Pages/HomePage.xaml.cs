using ClientUtilsProject.ViewModels;
using Serilog.Core;

namespace SportProject.Pages;

public partial class HomePage : ContentPage
{
    public HomeVM VM { get; set; }
    private Logger Logger { get; set; }

    public Color SixtyColor{ get; set; }
    
    public HomePage(HomeVM vm, Logger logger)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Logger = logger;
    }
}

public class StatutBarBehaviour  : Behavior<Page>
{
    public static readonly BindableProperty StatutBarColorProperty =
        BindableProperty.Create(
            nameof(StatutBarColor),
            typeof(Color),
            typeof(StatutBarBehaviour ),
            default(Color));

    public Color StatutBarColor
    {
        get => (Color)GetValue(StatutBarColorProperty);
        set => SetValue(StatutBarColorProperty, value);
    }

    public StatutBarBehaviour()
    {
        Application.Current.RequestedThemeChanged += (s, a) =>
        {
            if (Application.Current.Resources.TryGetValue("SixtyColor", out var ressource))
            {
                // Utiliser la ressource comme un dictionnaire, si elle est effectivement un IDictionary
                if (ressource is IDictionary<string, object> dictionnaire)
                {
                    // Récupérer une propriété spécifique
                    if (dictionnaire.TryGetValue("SixtyColor", out var valeur))
                    {
                        StatutBarColor = (Color)valeur;
                    }
                }
            }
        };
    }
}
