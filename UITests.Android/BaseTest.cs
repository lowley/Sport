﻿using Appium.Interfaces.Generic.SearchContext;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace UITests;

public abstract class BaseTest
{
    protected AppiumDriver App => AppiumSetup.App;

    // This could also be an extension method to AppiumDriver if you prefer
    protected AppiumElement FindUIElementByAutomationId(string id)
    {
        var realId = "sxb.sport:id/" + id;

        if (App is WindowsDriver)
        {
            return App.FindElement(MobileBy.AccessibilityId(realId));
        }

        return App.FindElement(MobileBy.Id(realId));
    }

    protected IList<AppiumElement> FindUIElementsByAutomationId(string id)
    {
        var realId = "sxb.sport:id/" + id;

        if (App is WindowsDriver)
        {
            return App.FindElements(MobileBy.AccessibilityId(realId));
        }

        return App.FindElements(MobileBy.Id(realId));
    }

    /***
     * Find an element by its XPath
     * @param id The XPath of the element
     * @return The element found
     * @throws InvalidDataException If the element is not found
     */
    protected AppiumElement FindUIElementByXPath(string id)
    {
        AppiumElement result = null;
        try
        {
            result = App.FindElement(MobileBy.XPath(id));
        }
        catch(Exception e)
        { }

        return result;
    }

    protected IReadOnlyCollection<AppiumElement> FindUIElementsByXPath(string id)
    {
        IReadOnlyCollection<AppiumElement> result = null;
        try
        {
            result = App.FindElements(MobileBy.XPath(id));
        }
        catch (Exception e)
        { }

        return result;
    }

    //[SetUp]
    //public void AfterEachTest()
    //{
    //    App.TerminateApp("sxb.sport");
    //}


    //[TearDown]
    //public void AfterEachTest()
    //{
    //    App.TerminateApp("sxb.sport");
    //}
}