using NUnit.Framework;
using ClientUtilsProject.Utils;
using LanguageExt;
using ClientUtilsProject.DataClasses;

namespace UITests
{
    public class HomePageTest : BaseTest
    {
        public const string RESUME_SESSION_BUTTON_TITLE = nameof(RESUME_SESSION_BUTTON_TITLE); 
        
        [Test]
        public void CreateSessionTest()
        {
            ClearDatas();

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
            ClearDatas();

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
            ClearDatas();

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
            
            //value : 11Kg
            var value = FindUIElementByAutomationId("ExerciseValue");
            value.Click();
            value.SendKeys("11");

            //cache le clavier
            var bouton4 = FindUIElementByAutomationId("HideKeyboardBtn");
            bouton4.Click();
            Task.Delay(500).Wait();
            
            var bouton2 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton2.Click();
            Task.Delay(500).Wait();

            //navigation
            var back = FindUIElementByAutomationId("BackButton");
            back.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title2 = FindUIElementByXPath(@"//android.widget.TextView[@text='HomePage'][1]");
            Assert.That(title2, Is.Not.Null);

            //navigation vers ExercicesPage pour vérification
            var bouton3 = FindUIElementByAutomationId("ExercisesBtn");
            bouton3.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title3 = FindUIElementByXPath(@"//android.widget.TextView[@text='Liste des exercices'][1]");
            Assert.That(title3, Is.Not.Null);

            var items = FindUIElementsByAutomationId("exercise");
            var numberOfItems = items.Count;
            Assert.That(numberOfItems, Is.EqualTo(1));

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }

        [Test]
        public void ListExercises_twoDifferentExercises_Test()
        {
            ClearDatas();

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
            
            //value : 11Kg
            var value = FindUIElementByAutomationId("ExerciseValue");
            value.Click();
            value.SendKeys("11");

            //cache le clavier
            var bouton6 = FindUIElementByAutomationId("HideKeyboardBtn");
            bouton6.Click();
            Task.Delay(500).Wait();
            
            var bouton2 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton2.Click();
            Task.Delay(500).Wait();

            //navigation
            var back = FindUIElementByAutomationId("BackButton");
            back.Click();
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
            
            //value : 11Kg
            var value2 = FindUIElementByAutomationId("ExerciseValue");
            value2.Click();
            value2.SendKeys("11");

            //cache le clavier
            var bouton7 = FindUIElementByAutomationId("HideKeyboardBtn");
            bouton7.Click();
            Task.Delay(500).Wait();
            
            var bouton4 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton4.Click();
            Task.Delay(500).Wait();

            //navigation
            var back2 = FindUIElementByAutomationId("BackButton");
            back2.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title4 = FindUIElementByXPath(@"//android.widget.TextView[@text='HomePage'][1]");
            Assert.That(title4, Is.Not.Null);

            //navigation vers ExercicesPage pour vérification
            var bouton5 = FindUIElementByAutomationId("ExercisesBtn");
            bouton5.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title5 = FindUIElementByXPath(@"//android.widget.TextView[@text='Liste des exercices'][1]");
            Assert.That(title5, Is.Not.Null);

            var items = FindUIElementsByAutomationId("exercise");
            var numberOfItems = items.Count;
            Assert.That(numberOfItems, Is.EqualTo(2));

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }

        [Test]
        public void ListExercises_threeDifficultiesForSameExercise_Test()
        {
            ClearDatas();

            //ajoute un exercice
            var bouton = FindUIElementByAutomationId("AddExerciseBtn");
            bouton.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title = FindUIElementByXPath(@"//android.widget.TextView[@text='ExercisePage'][1]");
            Assert.That(title, Is.Not.Null);

            //act
            var name = FindUIElementByAutomationId("ExerciseName");
            name.SendKeys("Crunch");
            //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

            //value : 11Kg
            var value = FindUIElementByAutomationId("ExerciseValue");
            value.Click();
            value.SendKeys("11");

            //cache le clavier
            var bouton3 = FindUIElementByAutomationId("HideKeyboardBtn");
            bouton3.Click();
            Task.Delay(500).Wait();

            var bouton2 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton2.Click();
            Task.Delay(500).Wait();

            //le nom n'a pas été effacé
            var name2 = FindUIElementByAutomationId("ExerciseName");
            Assert.That(name2.Text, Is.EqualTo("Crunch"));

            //value : 14Kg
            var value2 = FindUIElementByAutomationId("ExerciseValue");
            value2.Click();
            value2.SendKeys("14");

            //cache le clavier
            var bouton6 = FindUIElementByAutomationId("HideKeyboardBtn");
            bouton6.Click();
            Task.Delay(500).Wait();

            var bouton4 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton4.Click();
            Task.Delay(500).Wait();

            //le nom n'a pas été effacé
            var name5 = FindUIElementByAutomationId("ExerciseName");
            Assert.That(name5.Text, Is.EqualTo("Crunch"));

            //value : 17Kg
            var value3 = FindUIElementByAutomationId("ExerciseValue");
            value3.Click();
            value3.SendKeys("17");

            //cache le clavier
            var bouton7 = FindUIElementByAutomationId("HideKeyboardBtn");
            bouton7.Click();
            Task.Delay(500).Wait();

            var bouton8 = FindUIElementByAutomationId("SaveExerciseBtn");
            bouton8.Click();
            Task.Delay(500).Wait();

            //navigation
            var back = FindUIElementByAutomationId("BackButton");
            back.Click();
            Task.Delay(500).Wait();
            var title2 = FindUIElementByXPath(@"//android.widget.TextView[@text='HomePage'][1]");
            Assert.That(title2, Is.Not.Null);

            //navigation vers ExercicesPage pour vérification
            var bouton5 = FindUIElementByAutomationId("ExercisesBtn");
            bouton5.Click();
            Task.Delay(500).Wait();
            //verif navigation
            var title5 = FindUIElementByXPath(@"//android.widget.TextView[@text='Liste des exercices'][1]");
            Assert.That(title5, Is.Not.Null);

            var exercises = FindUIElementsByAutomationId("exercise");
            var numberOfExercises = exercises.Count;
            Assert.That(numberOfExercises, Is.EqualTo(1));

            var difficulties = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/difficulty'][1]/android.widget.TextView");
            
            var numberOfDifficulties = difficulties.Count;
            Assert.That(numberOfDifficulties, Is.EqualTo(3));

            var difficulty1 = difficulties.ElementAt(0);
            var difficulty2 = difficulties.ElementAt(1);
            var difficulty3 = difficulties.ElementAt(2);

            var difficulté_11 = new ExerciceDifficulty(11, "Kg");
            var difficulté_14 = new ExerciceDifficulty(14, "Kg");
            var difficulté_17 = new ExerciceDifficulty(17, "Kg");

            Assert.That(difficulty1.Text, Is.EqualTo(difficulté_11.ShowMeShort));
            Assert.That(difficulty2.Text, Is.EqualTo(difficulté_14.ShowMeShort));
            Assert.That(difficulty3.Text, Is.EqualTo(difficulté_17.ShowMeShort));

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }

        [Test]
        public void AddSessionPage_MenuTitleInHomePageBecomesReprendreSession()
        {
            ClearDatas();
            
            //ajoute une session
            var bouton = FindUIElementByAutomationId("AddSessionBtn");
            bouton.Click();
            Task.Delay(500).Wait();
            
            //stockage pour asserts plus tard
            var initialDate = FindUIElementByAutomationId("InitialDate").Text;
            var initialTime = FindUIElementByAutomationId("InitialTime").Text;
            
            //clique sur terminer session
            var bouton2 = FindUIElementByAutomationId("CloseSessionBtn");
            Assert.That(bouton2, Is.Not.Null);
            bouton2.Click();
            Task.Delay(500).Wait();
            
            // retour page d'accueil
            var title = FindUIElementByXPath(@"//android.widget.TextView[@text='HomePage'][1]");
            Assert.That(title, Is.Not.Null);
            
            //vérifie texte du bouton changé
            var bouton3 = FindUIElementByAutomationId("AddSessionBtn");
            Assert.That(bouton3, Is.Not.Null);
            Assert.That(bouton3.Text, Is.EqualTo(RESUME_SESSION_BUTTON_TITLE));
            
            bouton3.Click();
            Task.Delay(500).Wait();
            
            // page reprendre session
            var title2 = FindUIElementByXPath(@"//android.widget.TextView[@text='SessionPage'][1]");
            Assert.That(title2, Is.Not.Null);
            
            //nouvelles valeurs identiques?
            var initialDate2 = FindUIElementByAutomationId("InitialDate").Text;
            var initialTime2 = FindUIElementByAutomationId("InitialTime").Text;

            Assert.That(initialDate2, Is.EqualTo(initialDate));
            Assert.That(initialTime2, Is.EqualTo(initialTime));
            
            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }
        
        
        
        
        
        
        void ClearDatas()
        {
            var bouton = FindUIElementByAutomationId("ClearBtn");
            bouton.Click();
        }

    }
}
