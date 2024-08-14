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
    public ISportLogger LoggerFake { get; set; }
    
    [SetUp]
    public void Setup()
    {
        NavigationFake = A.Fake<ISportNavigation>();
        LoggerFake = A.Fake<ISportLogger>();
        RepositoryFake = A.Fake<ISportRepository>();
        SUT = new ExerciseVM(NavigationFake, RepositoryFake, LoggerFake);
    }

    [Test]
    public void add_save_simple_exercise()
    {
        //assert
        
        //todo vider base
        //ExercisesVM._exercices = [];
        SUT.CurrentExercise.ExerciseName = "test";
        var difficulty = new ExerciceDifficulty(15, "Kg");
        SUT.CurrentDifficulty = difficulty;


        //act
        SUT.Save();


        //arrange
        A.CallTo(() => RepositoryFake.AddAsync(
                A<Exercise>.That.Matches(e =>
                    e.ExerciseName.Equals("test") && 
                    e.ExerciseDifficulties.Count == 1 &&
                    e.ExerciseDifficulties[0].DifficultyLevel == 15 &&
                    e.ExerciseDifficulties[0].DifficultyName.Equals("Kg"))
                ))
            .MustHaveHappenedOnceExactly();
        
        /*Assert.AreEqual(1, ExercisesVM._exercices.Count);
        var savedExercise = ExercisesVM._exercices[0];
        Assert.AreEqual("test", savedExercise.ExerciseName);
        Assert.AreEqual(1, savedExercise.ExerciseDifficulties.Count);
        Assert.AreSame(difficulty, savedExercise.ExerciseDifficulties[0]);*/
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
        if(LoggerFake is not null)
            await LoggerFake.DisposeAsync();
    }
}