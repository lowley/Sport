using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using ClientUtilsProject.ViewModels;
using FakeItEasy;

namespace WIPClientTests
{
    public class Tests
    {
        public HomeVM SUT { get; set;}
        public ISportNavigation NavigationFake { get; set; }
        public ISportRepository RepoFake { get; set; }
        
        [SetUp]
        public void Setup()
        {
            RepoFake = A.Fake<ISportRepository>();
            NavigationFake = A.Fake<ISportNavigation>();
            SUT = new HomeVM(NavigationFake, RepoFake);
        }

        [Test]
        public void AddOneToZero_ProducesOne()
        {
            //assert

            //act

            //arrange
            Assert.AreEqual(1, 1);
        }
    }
}