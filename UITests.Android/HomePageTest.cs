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
            public void SessionPage_CreateAndCloseSession()
            {
                var EXERCISE_NAME = "Dips";
                var DIFFICULTY_VALUE = 11;
                
                ClearDatas();
                
                ClickButtonWithAutomationId("AddExerciseBtn");

                //EXERCISE PAGE
                //*************

                AssertPageTitleIs("ExercisePage");

                //act
                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys(EXERCISE_NAME);
                //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

                SetElementValueWithAutomationId("ExerciseValue", DIFFICULTY_VALUE);
                ClickButtonWithAutomationId("HideKeyboardBtn");
                ClickButtonWithAutomationId("SaveExerciseBtn");
                ClickButtonWithAutomationId("BackBtn");
                
                //HOME PAGE
                //*************
                
                ClickButtonWithAutomationId("AddSessionBtn");
                Task.Delay(1500).Wait();
                
                //SESSION PAGE
                //*************

                //stockage pour asserts plus tard
                var initialDate = FindUIElementByAutomationId("InitialDate").Text;
                var initialTime = FindUIElementByAutomationId("InitialTime").Text;

                //ajout exercices
                var exercises = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/ExercisesGroup'][1]/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                var exercise = exercises.FirstOrDefault(elt => elt.Text == EXERCISE_NAME);
                exercise.Click();
                Task.Delay(500).Wait();
                
                var difficulties = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/DifficultiesGroup']/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                Assert.That(difficulties.Count, Is.EqualTo(1));
                var difficulty = difficulties.FirstOrDefault(elt => elt.Text == DIFFICULTY_VALUE.ToString()+"Kg");
                difficulty.Click();
                Task.Delay(500).Wait();
                
                ClickButtonWithAutomationId("PlusOneBtn");
                ClickButtonWithAutomationId("CloseSessionBtn");
                
                
                //HOME PAGE
                //*************
                
                AssertPageTitleIs("Accueil");
                ClickButtonWithAutomationId("SessionsBtn");
                
                //SESSIONS PAGE
                //*************
                
                AssertPageTitleIs("Affichage des sessions");
                
                // existence de session avec bonne date et bonne heure?
                var sessions = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/session']");
                
                var numberOfSessions = sessions.Count;
                Assert.That(numberOfSessions, Is.EqualTo(1));
                
                //todo: temps de départ et de fin à vérifier
                AssertThatElementWithAutomationIdHasText("startTime", initialTime);
                
                var endTime = FindUIElementByAutomationId("endTime");
                Assert.That(endTime, Is.Not.Null);
                Assert.That(initialTime, Is.LessThan(endTime.Text));

                var isOpen = FindUIElementsByAutomationId("isOpened")[0];
                Assert.That(isOpen, Is.EqualTo("non"));
                
                AppiumSetup.App.TerminateApp("sxb.sport");
                AppiumSetup.App.ActivateApp("sxb.sport");
            }

            [Test]
            public void SessionPage_CreateAndKeepSession()
            {
                var EXERCISE_NAME = "Dips";
                var DIFFICULTY_VALUE = 11;

                ClearDatas();

                ClickButtonWithAutomationId("AddExerciseBtn");

                //EXERCISE PAGE
                //*************

                AssertPageTitleIs("ExercisePage");

                //act
                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys(EXERCISE_NAME);
                //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

                SetElementValueWithAutomationId("ExerciseValue", DIFFICULTY_VALUE);
                ClickButtonWithAutomationId("HideKeyboardBtn");
                ClickButtonWithAutomationId("SaveExerciseBtn");
                ClickButtonWithAutomationId("BackBtn");
                
                //HOME PAGE
                //*************
                
                ClickButtonWithAutomationId("AddSessionBtn");
                Task.Delay(1500).Wait();
                
                //SESSION PAGE
                //*************

                //stockage pour asserts plus tard
                var initialDate = FindUIElementByAutomationId("InitialDate").Text;
                var initialTime = FindUIElementByAutomationId("InitialTime").Text;

                //ajout exercices
                var exercises = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/ExercisesGroup'][1]/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                var exercise = exercises.FirstOrDefault(elt => elt.Text == EXERCISE_NAME);
                exercise.Click();
                Task.Delay(500).Wait();
                
                var difficulties = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/DifficultiesGroup']/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                Assert.That(difficulties.Count, Is.EqualTo(1));
                var difficulty = difficulties.FirstOrDefault(elt => elt.Text == DIFFICULTY_VALUE.ToString()+"Kg");
                difficulty.Click();
                Task.Delay(500).Wait();
                
                ClickButtonWithAutomationId("PlusOneBtn");
                ClickButtonWithAutomationId("ExitKeepSessionBtn");
                
                
                //HOME PAGE
                //*************
                
                AssertPageTitleIs("Accueil");
                ClickButtonWithAutomationId("SessionsBtn");
                
                //SESSIONS PAGE
                //*************
                
                AssertPageTitleIs("Affichage des sessions");
                
                // existence de session avec bonne date et bonne heure?
                var sessions = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/session']");
                
                var numberOfSessions = sessions.Count;
                Assert.That(numberOfSessions, Is.EqualTo(1));
                
                //todo: temps de départ et de fin à vérifier
                AssertThatElementWithAutomationIdHasText("startTime", initialTime);
                
                var endTime = FindUIElementByAutomationId("endTime");
                Assert.That(endTime, Is.Not.Null);
                Assert.That(initialTime, Is.EqualTo(endTime.Text));

                var isOpen = FindUIElementsByAutomationId("isOpened")[0];
                Assert.That(isOpen.Text, Is.EqualTo("oui"));
                
                AppiumSetup.App.TerminateApp("sxb.sport");
                AppiumSetup.App.ActivateApp("sxb.sport");
            }

            [Test]
            public void SessionPage_Create2SessionsChooseWhichOpened()
            {
                var EXERCISE_NAME = "Dips";
                var DIFFICULTY_VALUE = 11;

                ClearDatas();

                ClickButtonWithAutomationId("AddExerciseBtn");

                //EXERCISE PAGE
                //*************

                AssertPageTitleIs("ExercisePage");

                //act
                var name = FindUIElementByAutomationId("newExerciseName");
                name.SendKeys(EXERCISE_NAME);
                //var difficulty = FindUIElementByAutomationId("ExerciseDifficulty");

                SetElementValueWithAutomationId("ExerciseValue", DIFFICULTY_VALUE);
                ClickButtonWithAutomationId("HideKeyboardBtn");
                ClickButtonWithAutomationId("SaveExerciseBtn");
                ClickButtonWithAutomationId("BackBtn");

                //HOME PAGE
                //*************

                ClickButtonWithAutomationId("AddSessionBtn");
                Task.Delay(1500).Wait();

                //SESSION PAGE : 1st session
                //*************

                //stockage pour asserts plus tard
                var initialDate = FindUIElementByAutomationId("InitialDate").Text;
                var initialTime = FindUIElementByAutomationId("InitialTime").Text;

                //ajout exercices
                var exercises = FindUIElementsByXPath(
                    "//android.view.ViewGroup[@resource-id='sxb.sport:id/ExercisesGroup'][1]/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                var exercise = exercises.FirstOrDefault(elt => elt.Text == EXERCISE_NAME);
                exercise.Click();
                Task.Delay(500).Wait();

                var difficulties = FindUIElementsByXPath(
                    "//android.view.ViewGroup[@resource-id='sxb.sport:id/DifficultiesGroup']/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                Assert.That(difficulties.Count, Is.EqualTo(1));
                var difficulty = difficulties.FirstOrDefault(elt => elt.Text == DIFFICULTY_VALUE.ToString() + "Kg");
                difficulty.Click();
                Task.Delay(500).Wait();

                ClickButtonWithAutomationId("PlusOneBtn");
                ClickButtonWithAutomationId("CloseSessionBtn");

                //HOME PAGE
                //*************

                ClickButtonWithAutomationId("AddSessionBtn");
                Task.Delay(1500).Wait();

                //SESSION PAGE : 2nd session
                //*************

                //stockage pour asserts plus tard
                var initialDate2 = FindUIElementByAutomationId("InitialDate").Text;
                var initialTime2 = FindUIElementByAutomationId("InitialTime").Text;

                //ajout exercices
                var exercises2 = FindUIElementsByXPath(
                    "//android.view.ViewGroup[@resource-id='sxb.sport:id/ExercisesGroup'][1]/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                var exercise2 = exercises2.FirstOrDefault(elt => elt.Text == EXERCISE_NAME);
                exercise2.Click();
                Task.Delay(500).Wait();

                var difficulties2 = FindUIElementsByXPath(
                    "//android.view.ViewGroup[@resource-id='sxb.sport:id/DifficultiesGroup']/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.TextView");
                Assert.That(difficulties.Count, Is.EqualTo(1));
                var difficulty2 = difficulties2.FirstOrDefault(elt => elt.Text == DIFFICULTY_VALUE.ToString() + "Kg");
                difficulty2.Click();
                Task.Delay(500).Wait();

                ClickButtonWithAutomationId("PlusOneBtn");
                ClickButtonWithAutomationId("ExitKeepSessionBtn");

                //HOME PAGE
                //*************

                AssertPageTitleIs("Accueil");
                ClickButtonWithAutomationId("SessionsBtn");

                //SESSIONS PAGE
                //*************

                AssertPageTitleIs("Affichage des sessions");

                // existence de session correctement ouvertes/fermées?
                var sessions = FindUIElementsByXPath("//android.view.ViewGroup[@resource-id='sxb.sport:id/session']");

                var numberOfSessions = sessions.Count;
                Assert.That(numberOfSessions, Is.EqualTo(2));

                //les sessions sont triées de la plus récente à la plus ancienne
                var isOpen = FindUIElementsByAutomationId("isOpened")
                    .Reverse().ToList();
                Assert.That(isOpen[0].Text, Is.EqualTo("non"));
                Assert.That(isOpen[1].Text, Is.EqualTo("oui"));

                //modification session ouverte
                var openButtons = FindUIElementsByAutomationId("openSessionBtn")
                    .Reverse().ToList();
                openButtons[0].Click();

                Assert.That(isOpen[0].Text, Is.EqualTo("oui"));
                Assert.That(isOpen[1].Text, Is.EqualTo("non"));
                
            }

            [Test]
            public void ModifyExercice()
            {
                
                
                
                
            }
        }

        
    }
}