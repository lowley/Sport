using Geolocalisation = WIPClient.Utils.Geolocalisation;
using IGeolocalisation = WIPClient.Utils.IGeolocalisation;

namespace UnitTests
{
    public class LocalisationTests
    {
        public IGeolocalisation SUT { get; set; }

        [SetUp]
        public void Setup()
        {
            SUT = new Geolocalisation();

        }

        [Test]
        public async Task Constructor_bypassPermission_true()
        {
            //assert

            //act
            await SUT.VerifyPermission(true);

            //arrange
            Assert.NotNull(SUT);
        }

        [Test]
        public async Task Constructor_bypassPermission_false()
        {
            //assert
           
            //act
            try
            {
                await SUT.VerifyPermission(false);
            }
            catch(Exception ex)
            {
                //arrange
                Assert.NotNull(ex);
            }
        }



    }
}
