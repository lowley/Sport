using NUnit.Framework;
using ClientUtilsProject.Utils;
using LanguageExt;


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
            Assert.That(initialTime.Text, Is.AtLeast(DateTime.Now.AddMinutes(-1).TimeOfDay.ToString(SharedUtilDatas.HOUR_MINUTES_FORMAT)));

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }

            [Test]
        public void CreateExerciseTest()
        {
            // Arrange
            var bouton = FindUIElementByAutomationId("AddExerciseBtn");

            // Act
            bouton.Click();
            Task.Delay(500).Wait();

            // Assert
            var title = FindUIElementByXPath(@"//android.widget.TextView[@text='ExercisePage'][1]");
            Assert.That(title, Is.Not.Null);

            //var pageTitle = FindUIElement("PageTitle");
            //Assert.That(pageTitle.Text, Is.EqualTo("CreateSession"));

            var name = FindUIElementByAutomationId("ExerciseName");
            Assert.That(name, Is.Not.Null);
            var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");
            Assert.That(difficulty, Is.Not.Null);

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }

        [Test]
        public void ListExercises_one_Test()
        {
            //ajoute un exercice
            var bouton = FindUIElementByAutomationId("AddExerciseBtn");
            bouton.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title = FindUIElementByXPath(@"//android.widget.TextView[@text='ExercisePage'][1]");
            Assert.That(title, Is.Not.Null);

            //act
            var name = FindUIElementByAutomationId("ExerciseName");
            name.SendKeys("Dips");
            var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");
            //set to 75Kg by default
            difficulty.Click();
            var bouton2 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton2.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title2 = FindUIElementByXPath(@"//android.widget.TextView[@text='HomePage'][1]");
            Assert.That(title2, Is.Not.Null);

            //navigation vers ExercicesPage pour vérification
            var bouton3 = FindUIElementByAutomationId("ExercisesBtn");
            bouton3.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title3 = FindUIElementByXPath(@"//android.widget.TextView[@text='ExercisesPage'][1]");
            Assert.That(title3, Is.Not.Null);

            var items = FindUIElementsByAutomationId("exercise");
            var numberOfItems = items.Count;
            Assert.That(numberOfItems, Is.EqualTo(1));

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }

        [Test]
        public void ListExercises_two_Test()
        {
            //ajoute un exercice
            var bouton = FindUIElementByAutomationId("AddExerciseBtn");
            bouton.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title = FindUIElementByXPath(@"//android.widget.TextView[@text='ExercisePage'][1]");
            Assert.That(title, Is.Not.Null);

            //act
            var name = FindUIElementByAutomationId("ExerciseName");
            name.SendKeys("Dips");
            var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");
            //set to 75Kg by default
            difficulty.Click();
            var bouton2 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton2.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title2 = FindUIElementByXPath(@"//android.widget.TextView[@text='HomePage'][1]");
            Assert.That(title2, Is.Not.Null);

            //ajoute un exercice, objet de ce test
            var bouton3 = FindUIElementByAutomationId("AddExerciseBtn");
            bouton3.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title3 = FindUIElementByXPath(@"//android.widget.TextView[@text='ExercisePage'][1]");
            Assert.That(title3, Is.Not.Null);

            //act for this test
            var name2 = FindUIElementByAutomationId("ExerciseName");
            name2.SendKeys("Crunches");
            var difficulty2 = FindUIElementByAutomationId("ExerciseDifficulty");
            //on garde la value par défaut
            var bouton4 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton4.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title4 = FindUIElementByXPath(@"//android.widget.TextView[@text='HomePage'][1]");
            Assert.That(title4, Is.Not.Null);

            //navigation vers ExercicesPage pour vérification
            var bouton5 = FindUIElementByAutomationId("ExercisesBtn");
            bouton5.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title5 = FindUIElementByXPath(@"//android.widget.TextView[@text='ExercisesPage'][1]");
            Assert.That(title5, Is.Not.Null);

            var items = FindUIElementsByAutomationId("exercise");
            var numberOfItems = items.Count;
            Assert.That(numberOfItems, Is.EqualTo(2));

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
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
