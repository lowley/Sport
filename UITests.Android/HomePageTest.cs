using NUnit.Framework;


namespace UITests
{
    public class HomePageTest : BaseTest
    {
        [Test]
        public async Task CreateSessionTest()
        {
            // Arrange
            var bouton = FindUIElement("AddSessionBtn");
            var pageTitle = FindUIElement("PageTitle");
            var initialDate = FindUIElement("InitialDate");
            var initialTime = FindUIElement("InitialTime");

            // Act
            bouton.Click();
            Task.Delay(500).Wait();

            // Assert
            Assert.That(pageTitle.Text, Is.EqualTo("CreateSession"));
            Assert.That(initialDate.Text, Is.EqualTo(DateTime.Now.ToString("dd/MM/yyyy")));
            Assert.That(initialTime.Text, Is.AtLeast(DateTime.Now.AddSeconds(-2)));

        }

        public void ClickCounterTest()
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
