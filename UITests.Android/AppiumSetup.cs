using NUnit.Framework;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
    //lancer appium

    //IL Y A DES CHOSES A ADAPTER POUR QUE CA MARCHE
    //ici, et dans MainActivity, en fonction du fichier projet de l'appli android
    
    //il faut aussi lancer dotnet clean + dotnet build
    //dans l'appli le répertoire de l'appli android pour générer l' apk

    //appium -a 192.168.1.173 -p 4723 --session-override ,(appium suffit)
    
    //java.lang.SecurityException: Permission denial: writing to settings requires:android.permission.WRITE_SECURE_SETTINGS
    //adb kill-server
    //adb shell pm grant sxb.sport android.permission.WRITE_SECURE_SETTINGS


    private static AppiumDriver? driver;

    public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        AppiumServerHelper.StartAppiumLocalServer(port:4724);

        var androidOptions = new AppiumOptions
        {
            AutomationName = "UiAutomator2",
            PlatformName = "Android",   
            PlatformVersion = "13",
            App = "C:\\Users\\olivier\\source\\repos\\SportSolution\\SportProject\\bin\\Debug\\net8.0-android\\sxb.sport.apk",
            DeviceName = "alioth",
            //DeviceName = "OnePlus ONEPLUS A6013"
            

        };

        androidOptions.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, "true");
        androidOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, "false");
        androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "sxb.sport");

        //Make sure to set [Register("sxb.wipclient.MainActivity")] on the MainActivity of your android application
        androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, $"sxb.sport.MainActivity");
        
        driver = new AndroidDriver(androidOptions);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();

        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}