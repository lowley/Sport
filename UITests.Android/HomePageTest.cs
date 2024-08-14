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

            ClickButtonWithAutomationId("AddSessionBtn");
            AssertPageTitleIs("SessionPage");

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
            ClickButtonWithAutomationId("AddExerciseBtn");

            // Assert
            AssertPageTitleIs("ExercisePage");

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
            ClickButtonWithAutomationId("AddExerciseBtn");

            //EXERCISE PAGE
            //*************
            
            AssertPageTitleIs("ExercisePage");

            //act
            var name = FindUIElementByAutomationId("ExerciseName");
            name.SendKeys("Dips");
            var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");
            
            SetElementValueWithAutomationId("ExerciseValue", 11);
            ClickButtonWithAutomationId("HideKeyboardBtn");
            ClickButtonWithAutomationId("SaveExerciseBtn");
            ClickButtonWithAutomationId("BackBtn");

            //HOME PAGE
            //*************
            
            AssertPageTitleIs("Accueil");
            ClickButtonWithAutomationId("ExercisesBtn");

            //EXERCISES PAGE
            //*************
            
            AssertPageTitleIs("Liste des exercices");

            var items = FindUIElementsByAutomationId("exercise");
            var numberOfItems = items.Count;
            Assert.That(numberOfItems, Is.EqualTo(1));

            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }

        private void SetElementValueWithAutomationId(string automationId, int value)
        {
            var element = FindUIElementByAutomationId(automationId);
            element.Click();
            element.SendKeys(value.ToString());
        }

        [Test]
        public void ListExercises_twoDifferentExercises_Test()
        {
            ClearDatas();
            ClickButtonWithAutomationId("AddExerciseBtn");

            //EXERCISE PAGE
            //*************
            
            AssertPageTitleIs("ExercisePage");

            //act
            var name = FindUIElementByAutomationId("ExerciseName");
            name.SendKeys("Dips");
            var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");
            
            SetElementValueWithAutomationId("ExerciseValue", 11);
            ClickButtonWithAutomationId("HideKeyboardBtn");
            ClickButtonWithAutomationId("SaveExerciseBtn");
            ClickButtonWithAutomationId("BackBtn");

            //HOME PAGE
            //*************
            
            AssertPageTitleIs("Accueil");
            ClickButtonWithAutomationId("AddExerciseBtn");
            AssertPageTitleIs("ExercisePage");

            //EXERCISE PAGE
            //*************

            //act for this test
            var name2 = FindUIElementByAutomationId("ExerciseName");
            name2.SendKeys("Crunches");
            var difficulty2 = FindUIElementByAutomationId("ExerciseDifficulty");
            
            SetElementValueWithAutomationId("ExerciseValue", 11);
            ClickButtonWithAutomationId("HideKeyboardBtn");
            ClickButtonWithAutomationId("SaveExerciseBtn");
            ClickButtonWithAutomationId("BackBtn");

            //HOME PAGE
            //*************
            
            AssertPageTitleIs("Accueil");
            ClickButtonWithAutomationId("ExercisesBtn");
            AssertPageTitleIs("Liste des exercices");

            //EXERCISES PAGE
            //*************
            
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
            ClickButtonWithAutomationId("AddExerciseBtn");
            AssertPageTitleIs("ExercisePage");

            //EXERCISE PAGE
            //*************
            
            //act
            var name = FindUIElementByAutomationId("ExerciseName");
            name.SendKeys("Crunch");
            //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

            SetElementValueWithAutomationId("ExerciseValue", 11);
            ClickButtonWithAutomationId("HideKeyboardBtn");
            ClickButtonWithAutomationId("SaveExerciseBtn");

            //le nom n'a pas été effacé
            var name2 = FindUIElementByAutomationId("ExerciseName");
            Assert.That(name2.Text, Is.EqualTo("Crunch"));

            SetElementValueWithAutomationId("ExerciseValue", 14);
            ClickButtonWithAutomationId("HideKeyboardBtn");
            ClickButtonWithAutomationId("SaveExerciseBtn");

            //le nom n'a pas été effacé
            var name5 = FindUIElementByAutomationId("ExerciseName");
            Assert.That(name5.Text, Is.EqualTo("Crunch"));

            SetElementValueWithAutomationId("ExerciseValue", 17);
            
            ClickButtonWithAutomationId("HideKeyboardBtn");
            ClickButtonWithAutomationId("SaveExerciseBtn");
            ClickButtonWithAutomationId("BackBtn");

            //HOME PAGE
            //*************
            
            AssertPageTitleIs("Accueil");
            ClickButtonWithAutomationId("ExercisesBtn");
            AssertPageTitleIs("Liste des exercices");

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
            ClickButtonWithAutomationId("AddSessionBtn");
            
            //SESSION PAGE
            //*************
            
            //stockage pour asserts plus tard
            var initialDate = FindUIElementByAutomationId("InitialDate").Text;
            var initialTime = FindUIElementByAutomationId("InitialTime").Text;
            
            ClickButtonWithAutomationId("CloseSessionBtn");

            //HOME PAGE
            //*************
            
            AssertPageTitleIs("Accueil");
            ClickButtonWithAutomationId("SessionsBtn");

            //SESSIONS PAGE
            //*************
            
            AssertPageTitleIs("Affichage des sessions");
            
            // existence de session avec bonne date et bonne heure?
            var sessions = FindUIElementsByXPath("//android.widget.TextView[@resource-id='sxb.sport:id/session']");
            
            var numberOfSessions = sessions.Count;
            Assert.That(numberOfSessions, Is.EqualTo(1));
            
            //todo: temps de départ et de fin à vérifier
            var startTime = FindUIElementByAutomationId("startTime");
            Assert.That(startTime, Is.Not.Null);
            Assert.That(initialTime, Is.EqualTo(startTime.Text));
            
            var endTime = FindUIElementByAutomationId("endTime");;
            Assert.That(endTime, Is.Not.Null);

            Assert.That(initialTime, Is.LessThan(endTime.Text));
            
            AppiumSetup.App.TerminateApp("sxb.sport");
            AppiumSetup.App.ActivateApp("sxb.sport");
        }
        
        
        //***************************************************************
        
        private void ClickButtonWithAutomationId(string automationId)
        {
            var bouton = FindUIElementByAutomationId(automationId);
            bouton.Click();
            Task.Delay(500).Wait();
        }
        
        private void AssertPageTitleIs(string pageTitle)
        {
            var title = FindUIElementByXPath($@"//android.widget.TextView[@text='{pageTitle}'][1]");
            Assert.That(title, Is.Not.Null);
        }
        
        void ClearDatas()
        {
            var bouton = FindUIElementByAutomationId("ClearBtn");
            bouton.Click();
        }

    }
}
