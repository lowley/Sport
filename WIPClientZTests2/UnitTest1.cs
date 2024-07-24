using WIPClientVM2;

namespace WIPClientTests
{
    public class Tests
    {
        public MainPageVM SUT { get; set;}

        [SetUp]
        public void Setup()
        {
            SUT = new MainPageVM();

        }

        [Test]
        public void MainPageVM_AddOneToZero_ProducesOne()
        {
            //assert

            //act
            SUT.AddOne();


            //arrange
            Assert.AreEqual(1, SUT.Zcounter);
        }
    }
}