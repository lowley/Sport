using NUnit.Framework;
using ClientUtilsProject.Utils;


namespace UITests
{
    public class HomePageTest : BaseTest
    {
        [Test]
        public void CreateSessionTest()
        {
            // Arrange
            var bouton = FindUIElementByAutomationId("AddSessionBtn");

            // Act
            bouton.Click();
            Task.Delay(500).Wait();

            // Assert
            var title = FindUIElementByXPath(@"//android.widget.TextView[@text='SessionPage'][1]");
            Assert.That(title, Is.Not.Null);

            //var pageTitle = FindUIElement("PageTitle");
            //Assert.That(pageTitle.Text, Is.EqualTo("CreateSession"));

            var initialDate = FindUIElementByAutomationId("InitialDate");
            Assert.That(initialDate.Text, Is.EqualTo(DateTime.Now.ToString(SharedUtilDatas.COMPLETE_DATE_FORMAT)));
            var initialTime = FindUIElementByAutomationId("InitialTime");
            Assert.That(initialTime.Text, Is.AtLeast(DateTime.Now.AddMinutes(-1).TimeOfDay.ToString("T")));
        }

        public void ClickCounter()
        {
            // Arrange
            // Find elements with the value of the AutomationId property
            var bouton = FindUIElementByAutomationId("CounterBtn");

            // Act
            bouton.Click();
            Task.Delay(500).Wait(); // Wait for the click to register and show up on the screenshot

            // Assert
            //App.GetScreenshot().SaveAsFile($"{nameof(ClickCounterTest)}.png");
            var label = FindUIElementByAutomationId("CounterLabel");
            Assert.That(label.Text, Is.EqualTo("1"));
        }
    }
}
