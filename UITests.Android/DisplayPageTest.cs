using NUnit.Framework;


namespace UITests
{
    public class DisplayPageTest : BaseTest
    {
        [Test]
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
