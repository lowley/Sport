using WIPClientVM;

namespace WIPClientTests
{
    public class Tests
    {
        public DisplayVM SUT { get; set;}

        [SetUp]
        public void Setup()
        {
            SUT = new DisplayVM();

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