using NUnit.Framework;
using ClientUtilsProject.Utils;
using LanguageExt;
using ClientUtilsProject.DataClasses;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace UITests
{
    public class InterfaceTests
    {
        public class InitialTests : BaseTest
        {
            [Test]
            public void CreateSessionTest()
            {
                ClearDatas();

                ClickButtonWithAutomationId("AddSessionBtn");
                AssertPageTitleIs("SessionPage");

                AssertThatElementWithAutomationIdHasText("InitialDate",
                    DateTime.Now.ToString(SharedUtilDatas.COMPLETE_DATE_FORMAT));

                var initialTime = FindUIElementByAutomationId("InitialTime");
                Assert.That(initialTime.Text,
                    Is.AtLeast(DateTime.Now.AddMinutes(-1).TimeOfDay.ToString(SharedUtilDatas.HOUR_MINUTES_FORMAT)));

                AppiumSetup.App.TerminateApp("sxb.sport");
                AppiumSetup.App.ActivateApp("sxb.sport");
            }

            [Test]
            public void CreateExerciseTest()
            {
                ClearDatas();
                ClickButtonWithAutomationId("AddExerciseBtn");

                AssertPageTitleIs("ExercisePage");
                AssertThatElementWithAutomationIdIsNotNull("newExerciseName");
                AssertThatElementWithAutomationIdIsNotNull("existingExerciseName");
                AssertThatElementWithAutomationIdIsNotNull("ExerciseDifficulty");

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
                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys("Dips");
                //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

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

            [Test]
            public void ListExercises_twoDifferentExercises_Test()
            {
                ClearDatas();
                ClickButtonWithAutomationId("AddExerciseBtn");

                //EXERCISE PAGE
                //*************

                AssertPageTitleIs("ExercisePage");

                //act
                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys("Dips");
                //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

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
                var name2 = FindUIElementByAutomationId("newExerciseName");
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
                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys("Crunch");
                //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

                SetElementValueWithAutomationId("ExerciseValue", 11);
                ClickButtonWithAutomationId("HideKeyboardBtn");
                ClickButtonWithAutomationId("SaveExerciseBtn");

                //le nom n'a pas été effacé
                AppiumElement dropdown = FindUIElementByAutomationId("ExerciseDropdown");
                Assert.That(dropdown.Text, Is.EqualTo(new Exercise()
                {
                    ExerciseName = "Crunch"
                }.ToString()));

                SetElementValueWithAutomationId("ExerciseValue", 14);
                Task.Delay(500).Wait();
                ClickButtonWithAutomationId("HideKeyboardBtn");

                try
                {
                    ClickButtonWithAutomationId("SaveExerciseBtn");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Task.Delay(500).Wait();

                //le nom n'a pas été effacé
                dropdown = FindUIElementByAutomationId("ExerciseDropdown");
                Assert.That(dropdown.Text, Is.EqualTo(new Exercise()
                {
                    ExerciseName = "Crunch"
                }.ToString()));
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

                var difficulties =
                    FindUIElementsByXPath(
                        "//android.view.ViewGroup[@resource-id='sxb.sport:id/difficulty'][1]/android.widget.TextView");
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
                AssertThatElementWithAutomationIdHasText("startTime", initialTime);

                var endTime = FindUIElementByAutomationId("endTime");
                ;
                Assert.That(endTime, Is.Not.Null);

                Assert.That(initialTime, Is.LessThan(endTime.Text));

                AppiumSetup.App.TerminateApp("sxb.sport");
                AppiumSetup.App.ActivateApp("sxb.sport");
            }

            [Test]
            public void AddExercice_ExitThenEditName_Test()
            {
                var ExerciseNameBefore = "Crunch";
                var ExerciseNameAfter = "Crunches";

                ClearDatas();
                ClickButtonWithAutomationId("AddExerciseBtn");
                AssertPageTitleIs("ExercisePage");

                //EXERCISE PAGE
                //*************

                //act
                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys(ExerciseNameBefore);
                //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

                SetElementValueWithAutomationId("ExerciseValue", 11);
                ClickButtonWithAutomationId("HideKeyboardBtn");
                ClickButtonWithAutomationId("SaveExerciseBtn");
                ClickButtonWithAutomationId("BackBtn");

                //HOME PAGE
                //*************

                AssertPageTitleIs("Accueil");
                ClickButtonWithAutomationId("AddExerciseBtn");

                //EXERCISE PAGE
                //*************

                AppiumElement dropdown = FindUIElementByAutomationId("ExerciseDropdown");
                dropdown.Click();
                Task.Delay(500).Wait();

                //dropdown.SendKeys($"{ExerciseNameBefore.First()}");
                var dropdownItems =
                    FindUIElementsByXPath(
                        "//android.widget.ListView[@resource-id='sxb.sport:id/select_dialog_listview'][1]/android.widget.TextView");
                var item = dropdownItems.ElementAt(0);
                item.Click();

                // var dropdownSelect = new SelectElement(dropdown);
                // dropdownSelect.SelectByValue(ExerciseNameBefore);

                var difficulties =
                    FindUIElementsByXPath(
                        "//android.view.ViewGroup[@resource-id='sxb.sport:id/selectedDifficulty'][1]/android.widget.TextView");
                var numberOfDifficulties = difficulties.Count;
                Assert.That(numberOfDifficulties, Is.EqualTo(1));
                var difficulty1 = difficulties.ElementAt(0);
                var difficulté_11 = new ExerciceDifficulty(11, "Kg");
                Assert.That(difficulty1.Text, Is.EqualTo(difficulté_11.ShowMeShort));

                AppiumElement selectedName = FindUIElementByAutomationId("existingExerciseName");
                selectedName.SendKeys(ExerciseNameAfter);

                ClickButtonWithAutomationId("HideKeyboardBtn");
                ClickButtonWithAutomationId("SaveExerciseBtn");
                ClickButtonWithAutomationId("BackBtn");

                ClickButtonWithAutomationId("ExercisesBtn");
                AssertPageTitleIs("Liste des exercices");
                var items = FindUIElementsByAutomationId("exercise");
                var numberOfItems = items.Count;
                Assert.That(numberOfItems, Is.EqualTo(1));
                Assert.That(items[0].Text, Is.EqualTo(ExerciseNameAfter));

                AppiumSetup.App.TerminateApp("sxb.sport");
                AppiumSetup.App.ActivateApp("sxb.sport");
            }
        }

        public class SessionTests : BaseTest
        {

            [Test]
            public void Add1Serie1repetition()
            {
                var EXERCISE_NAME = "Dips";
                var EXERCISE_DIFFICULTY = 11;
                
                ClearDatas();
                ClickButtonWithAutomationId("AddExerciseBtn");

                //EXERCISE PAGE
                //*************
                AssertPageTitleIs("ExercisePage");

                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys(EXERCISE_NAME);
                SetElementValueWithAutomationId("ExerciseValue", EXERCISE_DIFFICULTY);
                ClickButtonWithAutomationId("HideKeyboardBtn");
                ClickButtonWithAutomationId("SaveExerciseBtn");
                ClickButtonWithAutomationId("BackBtn");

                //HOME PAGE
                //*************
                AssertPageTitleIs("Accueil");

                ClickButtonWithAutomationId("AddSessionBtn");

                //SESSION PAGE : ajout proprement-dit
                //*************
                AssertPageTitleIs("SessionPage");

                ClickButtonWithAutomationId(EXERCISE_NAME);
                ClickButtonWithAutomationId(EXERCISE_DIFFICULTY.ToString());
                ClickButtonWithAutomationId("+1");
                
                var difficulties = FindUIElementsByXPath(
                    "//android.view.ViewGroup[@resource-id='sxb.sport:id/ligne_serie'][1]/android.widget.TextView");
                Assert.That(difficulties.Count, Is.EqualTo(1));
                
                
                
                
                
                
                
                
            }
            
            
            
        }
    }
}