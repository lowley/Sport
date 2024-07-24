using System;
using System.Diagnostics;
using Xunit;

namespace WIPUnitTests
{
    public class UnitTests
    {
       

        public UnitTests()
        {
           
        }

        [Fact]
        public void SuccessfulTest()
        {
            Assert.True(true);
        }

        [Fact(Skip = "This test is skipped.")]
        public void SkippedTest()
        {
        }

        [Fact]
        public void FailingTest()
        {
            throw new Exception("This is meant to fail.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ParameterizedTest(int number)
        {
            Assert.NotEqual(0, number);
        }

        [Fact]
        public void OutputTest()
        {
            Trace.WriteLine("This is test output.");
        }

        [Fact]
        public void FailingOutputTest()
        {
            Trace.WriteLine("This is test output.");
            throw new Exception("This is meant to fail.");
        }
    }

}
