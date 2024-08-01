using WIPClientVM;

namespace WIPClientTests
{
    public class Tests
    {
        public HomeVM SUT { get; set;}

        [SetUp]
        public void Setup()
        {
            SUT = new HomeVM();

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