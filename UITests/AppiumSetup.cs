using NUnit.Framework;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
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
            App = "sxb.wipclient!App",
        };

        androidOptions.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, "true");
        androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "sxb.wipclient");

        //Make sure to set [Register("com.companyname.basicappiumsample.MainActivity")] on the MainActivity of your android application
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