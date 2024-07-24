using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIPClient.Utils;
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
        public void AddOneToZero_ProducesOne()
        {
            //assert

            //act

            //arrange
            Assert.AreEqual(1, 1);
        }
    }
}
