using NUnit.Framework;


namespace UITests
{
    public class HomePageTest : BaseTest
    {
        
        [Test]
        public void CreateSessionTest()
        {
            // Arrange
            var bouton = FindUIElement("AddSessionBtn");

            // Act
            bouton.Click();
            Task.Delay(500).Wait();

            // Assert
            var title = FindUIElementByXPath(@"//android.widget.TextView[@text='CreateSession'][1]");
            Assert.That(title, Is.Not.Null);

            //var pageTitle = FindUIElement("PageTitle");
            //Assert.That(pageTitle.Text, Is.EqualTo("CreateSession"));

            var initialDate = FindUIElement("InitialDate");
            Assert.That(initialDate.Text, Is.EqualTo(DateTime.Now.ToString("dd/MM/yyyy")));
            var initialTime = FindUIElement("InitialTime");
            Assert.That(initialTime.Text, Is.AtLeast(DateTime.Now.AddSeconds(-2)));

        }

        public void ClickCounter()
        {
            // Arrange
            // Find elements with the value of the AutomationId property
            var bouton = FindUIElement("CounterBtn");

            // Act
            bouton.Click();
            Task.Delay(500).Wait(); // Wait for the click to register and show up on the screenshot

            // Assert
            //App.GetScreenshot().SaveAsFile($"{nameof(ClickCounterTest)}.png");
            var label = FindUIElement("CounterLabel");
            Assert.That(label.Text, Is.EqualTo("1"));
        }
    }
}
