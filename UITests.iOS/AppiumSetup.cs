using NUnit.Framework;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
	private static AppiumDriver? driver;

	public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

	[OneTimeSetUp]
	public void RunBeforeAnyTests()
	{
		var iOSOptions = new AppiumOptions
		{
			// Specify XCUITest as the driver, typically don't need to change this
			AutomationName = "XCUITest",
			// Always iOS for iOS
			PlatformName = "iOS",
			// iOS Version
			PlatformVersion = "17.0",
			// Don't specify if you don't want a specific device
			DeviceName = "iPhone 15 Pro",
			// The full path to the .app file to test or the bundle id if the app is already installed on the device
			App = "com.companyname.basicappiumsample",
		};

		// Note there are many more options that you can use to influence the app under test according to your needs

		driver = new IOSDriver(iOSOptions);
	}

	[OneTimeTearDown]
	public void RunAfterAnyTests()
	{
		driver?.Quit();
	}
}