using ClientUtilsProject.Utils;
using ClientUtilsProject.ViewModels;
using FakeItEasy;

namespace WIPClientTests
{
    public class Tests
    {
        public HomeVM SUT { get; set;}
        public ISportNavigation NavigationFake { get; set; }
        
        [SetUp]
        public void Setup()
        {
            NavigationFake = A.Fake<ISportNavigation>();
            SUT = new HomeVM(NavigationFake);
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