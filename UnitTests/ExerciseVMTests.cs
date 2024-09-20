using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using ClientUtilsProject.ViewModels;
using FakeItEasy;
using Serilog.Core;

namespace WIPClientTests;

public class ExerciseVMTests
{
    public ExerciseVM SUT { get; set; }
    public ISportNavigation NavigationFake { get; set; }
    public ISportRepository RepositoryFake { get; set; }
    public SportContext ContextFake { get; set; }
    public ISportLogger LoggerFake { get; set; }

    [SetUp]
    public void Setup()
    {
        NavigationFake = A.Fake<ISportNavigation>();
        LoggerFake = A.Fake<ISportLogger>();
        RepositoryFake = A.Fake<ISportRepository>();
        ContextFake = A.Fake<SportContext>();
        SUT = new ExerciseVM(NavigationFake, RepositoryFake, LoggerFake);
    }

    [Test]
    public void add_save_simple_exercise__nothingInDatabase()
    {
        var NEW_EXERCISE_NAME = "test";
        var DIFFICULTY_LEVEL = 15;
        string DIFFICULTY_NAME = "Kg";
        
        //arrange
        SUT.Exercises = new ObservableCollection<Exercise>();

        SUT.NewExerciseName = NEW_EXERCISE_NAME;
        var difficulty = new ExerciceDifficulty(DIFFICULTY_LEVEL, DIFFICULTY_NAME);
        SUT.CurrentDifficulty = difficulty;

        A.CallTo(() => RepositoryFake.GetContext())
            .Returns(null);

        //addition & récupération éxercice sauvegardé
        var savedExercise = A.Captured<Exercise>();
        A.CallTo(() => RepositoryFake.AddAsync(
            savedExercise._
        )).ReturnsLazily(() => savedExercise.GetLastValue());

        var savedDifficulty = A.Captured<ExerciceDifficulty>();
        A.CallTo(() => RepositoryFake.AddAsync(
            savedDifficulty._
        )).ReturnsLazily(() => savedDifficulty.GetLastValue());

        //act
        SUT.Save();

        //assert
        A.CallTo(() => RepositoryFake.AddAsync(
                savedExercise.GetLastValue()))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => RepositoryFake.AddAsync(
                savedDifficulty.GetLastValue()))
            .MustHaveHappenedOnceExactly();
        
        Assert.That(savedExercise.Values[0].ExerciseName, Is.EqualTo(NEW_EXERCISE_NAME));
        Assert.That(savedExercise.Values[0].ExerciseDifficulties.Count, Is.EqualTo(0));
        
        Assert.That(savedDifficulty.Values[0].DifficultyLevel, Is.EqualTo(DIFFICULTY_LEVEL));
        
        Assert.That(savedDifficulty.Values[0].DifficultyName
            .Match(
                s => s,
                () => DIFFICULTY_NAME+"error"
                ), Is.EqualTo(DIFFICULTY_NAME));
    }

    [Test]
    public void add_save_simple_exercise__anotherExerciseInDatabase()
    {
        var NEW_EXERCISE_NAME = "test";
        var OLD_EXERCISE_NAME = NEW_EXERCISE_NAME + "2";
        var DIFFICULTY_LEVEL = 15;
        string DIFFICULTY_NAME = "Kg";
        
        //arrange
        var difficulty = new ExerciceDifficulty(DIFFICULTY_LEVEL, DIFFICULTY_NAME);
        SUT.CurrentDifficulty = difficulty;

        A.CallTo(() => RepositoryFake.GetContext())
            .Returns(null);

        //addition & récupération exercice sauvegardé
        var savedExercise = A.Captured<Exercise>();
        A.CallTo(() => RepositoryFake.AddAsync(
            savedExercise._
        )).ReturnsLazily(() => savedExercise.GetLastValue());

        var savedDifficulty = A.Captured<ExerciceDifficulty>();
        A.CallTo(() => RepositoryFake.AddAsync(
            savedDifficulty._
        )).ReturnsLazily(() => savedDifficulty.GetLastValue());

        SUT.Exercises = new ObservableCollection<Exercise>() { new Exercise()
        {
            Id = Guid.NewGuid(),
            ExerciseName = OLD_EXERCISE_NAME,
            ExerciseDifficulties = []
        } };
        
        SUT.NewExerciseName = NEW_EXERCISE_NAME;
        
        //act
        SUT.Save();

        //assert
        A.CallTo(() => RepositoryFake.AddAsync(
                savedExercise.GetLastValue()))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => RepositoryFake.AddAsync(
                savedDifficulty.GetLastValue()))
            .MustHaveHappenedOnceExactly();
        
        Assert.That(savedExercise.Values[0].ExerciseName, Is.EqualTo(NEW_EXERCISE_NAME));
        Assert.That(savedExercise.Values[0].ExerciseDifficulties.Count, Is.EqualTo(0));
        
        Assert.That(savedDifficulty.Values[0].ExerciceId, Is.EqualTo(savedExercise.Values[0].Id));
        Assert.That(savedDifficulty.Values[0].DifficultyLevel, Is.EqualTo(DIFFICULTY_LEVEL));
        Assert.That(savedDifficulty.Values[0].DifficultyName
            .Match(
                s => s,
                () => DIFFICULTY_NAME+"error"
            ), Is.EqualTo(DIFFICULTY_NAME));
    }

    [Test]
    public void add_save_simple_exercise__sameExerciseAndSameDifficultyInDatabase()
    {
        var NEW_EXERCISE_NAME = "test";
        var DIFFICULTY_LEVEL = 15;
        string DIFFICULTY_NAME = "Kg";
        
        //arrange
        var difficulty = new ExerciceDifficulty(DIFFICULTY_LEVEL, DIFFICULTY_NAME);
        SUT.CurrentDifficulty = difficulty;

        A.CallTo(() => RepositoryFake.GetContext())
            .Returns(null);

        //addition & récupération éxercice sauvegardé
        var savedExercise = A.Captured<Exercise>();
        A.CallTo(() => RepositoryFake.AddAsync(
            A<Exercise>._
        )).ReturnsLazily(() => savedExercise.GetLastValue());

        var savedDifficulty = A.Captured<ExerciceDifficulty>();
        A.CallTo(() => RepositoryFake.AddAsync(
            A<ExerciceDifficulty>._
        )).ReturnsLazily(() => savedDifficulty.GetLastValue());

        SUT.Exercises = new ObservableCollection<Exercise>() { new Exercise()
        {
            Id = Guid.NewGuid(),
            ExerciseName = NEW_EXERCISE_NAME,
            ExerciseDifficulties = {difficulty}
        } };
        
        SUT.NewExerciseName = NEW_EXERCISE_NAME;
        
        //act
        SUT.Save();

        //assert
        A.CallTo(() => RepositoryFake.AddAsync(
            A<Exercise>._)).MustNotHaveHappened();

        A.CallTo(() => RepositoryFake.AddAsync(
            A<ExerciceDifficulty>._)).MustNotHaveHappened();
    }

    [Test]
    public void add_save_simple_exercise__sameExerciseAndAnotherNameInDatabase()
    {
        var NEW_EXERCISE_NAME = "test";
        var OLD_EXERCISE_NAME = NEW_EXERCISE_NAME + "2";
        var DIFFICULTY_LEVEL = 15;
        string DIFFICULTY_NAME = "Kg";
        
        //arrange
        var difficulty = new ExerciceDifficulty(DIFFICULTY_LEVEL, DIFFICULTY_NAME);
        SUT.CurrentDifficulty = difficulty;

        A.CallTo(() => RepositoryFake.GetContext())
            .Returns(null);

        var savedExercise = new Exercise()
        {
            Id = Guid.NewGuid(),
            ExerciseName = NEW_EXERCISE_NAME,
            ExerciseDifficulties = { difficulty }
        };
        
        //addition & récupération exercice sauvegardé
        var exerciseToSave = A.Captured<Exercise>();
        A.CallTo(() => RepositoryFake.AddAsync(
            exerciseToSave._
        )).ReturnsLazily(() => savedExercise);

        var savedDifficulty = A.Captured<ExerciceDifficulty>();
        A.CallTo(() => RepositoryFake.AddAsync(
            savedDifficulty._
        )).ReturnsLazily(() => savedDifficulty.GetLastValue());

        SUT.Exercises = new ObservableCollection<Exercise>() { new Exercise()
        {
            Id = savedExercise.Id,
            ExerciseName = OLD_EXERCISE_NAME,
            ExerciseDifficulties = []
        } };
        
        SUT.NewExerciseName = NEW_EXERCISE_NAME;
        
        //act
        SUT.Save();

        //assert
        A.CallTo(() => RepositoryFake.AddAsync(
                A<Exercise>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => RepositoryFake.AddAsync(
                savedDifficulty.GetLastValue()))
            .MustHaveHappenedOnceExactly();
        
        Assert.That(exerciseToSave.Values[0].ExerciseName, Is.EqualTo(NEW_EXERCISE_NAME));
        Assert.That(exerciseToSave.Values[0].ExerciseDifficulties.Count, Is.EqualTo(0));
        
        Assert.That(savedDifficulty.Values[0].DifficultyLevel, Is.EqualTo(DIFFICULTY_LEVEL));
        
        Assert.That(savedDifficulty.Values[0].DifficultyName
            .Match(
                s => s,
                () => DIFFICULTY_NAME+"error"
            ), Is.EqualTo(DIFFICULTY_NAME));
    }

    [Test]
    public void add_save_simple_exercise__anotherExerciseWithSameNameInDatabase()
    {
        var NEW_EXERCISE_NAME = "test";
        var OLD_EXERCISE_NAME = NEW_EXERCISE_NAME + "2";
        var DIFFICULTY_LEVEL = 15;
        string DIFFICULTY_NAME = "Kg";
        
        //arrange
        var difficulty = new ExerciceDifficulty(DIFFICULTY_LEVEL, DIFFICULTY_NAME);
        SUT.CurrentDifficulty = difficulty;

        A.CallTo(() => RepositoryFake.GetContext())
            .Returns(null);

        //addition & récupération éxercice sauvegardé
        var savedExercise = A.Captured<Exercise>();
        A.CallTo(() => RepositoryFake.AddAsync(
            A<Exercise>._
        )).ReturnsLazily(() => savedExercise.GetLastValue());

        var savedDifficulty = A.Captured<ExerciceDifficulty>();
        A.CallTo(() => RepositoryFake.AddAsync(
            A<ExerciceDifficulty>._
        )).ReturnsLazily(() => savedDifficulty.GetLastValue());

        SUT.Exercises = new ObservableCollection<Exercise>() { new Exercise()
        {
            Id = Guid.NewGuid(),
            ExerciseName = NEW_EXERCISE_NAME,
            ExerciseDifficulties = []
        } };
        
        SUT.NewExerciseName = NEW_EXERCISE_NAME;
        
        //act
        SUT.Save();

        //assert
        A.CallTo(() => RepositoryFake.AddAsync(
            A<Exercise>._)).MustNotHaveHappened();

        A.CallTo(() => RepositoryFake.AddAsync(
            A<ExerciceDifficulty>._)).MustNotHaveHappened();
    }

    /*
    [Test]
    public void add_save_two_different_exercises()
    {
        //assert1
        ExercisesVM._exercices = [];
        SUT._currentExerciseName = "test";
        var difficulty1 = new ExerciceDifficulty(15, "Kg");
        SUT._currentDifficulty = difficulty1;


        //act1
        SUT.Save();

        //assert2
        SUT._currentExerciseName = "test2";
        var difficulty2 = new ExerciceDifficulty(10, "Kg");
        SUT._currentDifficulty = difficulty2;

        //act2
        SUT.Save();


        //arrange
        //2 exercices
        Assert.AreEqual(2, ExercisesVM._exercices.Count);
        var firstExercise = ExercisesVM._exercices[0];
        var secondExercise = ExercisesVM._exercices[1];

        //premier exercice
        Assert.AreEqual("test", firstExercise.ExerciseName);
        Assert.AreEqual(1, firstExercise.ExerciseDifficulties.Count);
        Assert.AreSame(difficulty1, firstExercise.ExerciseDifficulties[0]);

        //second exercice
        Assert.AreEqual("test2", secondExercise.ExerciseName);
        Assert.AreEqual(1, secondExercise.ExerciseDifficulties.Count);
        Assert.AreSame(difficulty2, secondExercise.ExerciseDifficulties[0]);
    }

    [Test]
    public void add_save_same_exercise_two_difficulties()
    {
        //assert1
        ExercisesVM._exercices = [];
        SUT._currentExerciseName = "test";
        var difficulty1 = new ExerciceDifficulty(15, "Kg");
        SUT._currentDifficulty = difficulty1;


        //act1
        SUT.Save();

        //assert2
        var difficulty2 = new ExerciceDifficulty(10, "Kg");
        SUT._currentDifficulty = difficulty2;

        //act2
        SUT.Save();


        //arrange
        //2 exercices
        Assert.AreEqual(1, ExercisesVM._exercices.Count);
        var firstExercise = ExercisesVM._exercices[0];

        //premier exercice
        Assert.AreEqual("test", firstExercise.ExerciseName);
        Assert.AreEqual(2, firstExercise.ExerciseDifficulties.Count);
        Assert.AreSame(difficulty1, firstExercise.ExerciseDifficulties[0]);
        Assert.AreSame(difficulty2, firstExercise.ExerciseDifficulties[1]);
    }

    [Test]
    public void add_save_same_exercises_with_same_difficulty__do_nothing()
    {
        //assert1
        ExercisesVM._exercices = [];
        SUT._currentExerciseName = "test";
        var difficulty1 = new ExerciceDifficulty(15,, "Kg");
        SUT._currentDifficulty = difficulty1;


        //act1
        SUT.Save();

        //assert2
        SUT._currentDifficulty = difficulty1;

        //act2
        SUT.Save();


        //arrange
        //2 exercices
        Assert.AreEqual(1, ExercisesVM._exercices.Count);
        var firstExercise = ExercisesVM._exercices[0];


        //premier exercice
        Assert.AreEqual("test", firstExercise.ExerciseName);
        Assert.AreEqual(1, firstExercise.ExerciseDifficulties.Count);
        Assert.AreSame(difficulty1, firstExercise.ExerciseDifficulties[0]);
    }*/

    [TearDown]
    public async Task End()
    {
        if (RepositoryFake is not null)
            await RepositoryFake.DisposeAsync();
        if (LoggerFake is not null)
            await LoggerFake.DisposeAsync();
        if (ContextFake is not null)
            await ContextFake.DisposeAsync();
    }
}