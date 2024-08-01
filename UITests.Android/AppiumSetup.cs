using NUnit.Framework;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
    //IL Y A DES CHOSES A ADAPTER POUR QUE CA MARCHE
    //ici, et dans MainActivity, en fonction du ficheir projet de l'appli android
    
    //il faut aussi lancer dotnet clean + dotnet build
    //dans l'appli le répertoire de l'appli android pour générer l' apk

    //appium -a 192.168.1.173 -p 4723 --session-override ,(appium suffit)
    //java.lang.SecurityException: Permission denial: writing to settings requires:android.permission.WRITE_SECURE_SETTINGS

    //remplacer sxb.wipclient par le nom de l' application (trouvable dans le csproj)
    //adb shell pm grant sxb.wipclient android.permission.WRITE_SECURE_SETTINGS


    private static AppiumDriver? driver;

    public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        AppiumServerHelper.StartAppiumLocalServer();

        var androidOptions = new AppiumOptions
        {
            AutomationName = "UiAutomator2",
            PlatformName = "Android",
            App = "C:\\Users\\olivier\\source\\repos\\WhoIsPerestroikan\\WIPClient\\bin\\Debug\\net8.0-android\\sxb.wipclient-Signed.apk",
            DeviceName = "alioth"
        };

        androidOptions.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, "true");
        androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "sxb.wipclient");

        //Make sure to set [Register("sxb.wipclient.MainActivity")] on the MainActivity of your android application
        androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, $"sxb.wipclient.MainActivity");

        driver = new AndroidDriver(androidOptions);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();

        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}