using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octono.Xrm.Tasks;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class SolutionVersionFormatterTests
    {
        [TestMethod]
        public void CanIncrementRevisionNumberFromValidInput()
        {
            var formatter = new SolutionVersionFormatter();
            string actual = formatter.Increment("0.0.0.0");
            Assert.AreEqual("0.0.0.1",actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsFormatExceptionWhenVersionNumberInvalid()
        {
            var formatter = new SolutionVersionFormatter();
            formatter.Increment("ABC");
        }

        [TestMethod]
        public void SupportsIncrementMajorMinorVersionNumber()
        {
            var formatter = new SolutionVersionFormatter();
            string actual = formatter.Increment("1.0");
            Assert.AreEqual("1.1", actual);
        }

        [TestMethod]
        public void CreatesNewFourPartVersionWhenInputIsEmptyString()
        {
            var formatter = new SolutionVersionFormatter();
            string actual = formatter.Increment("");
            Assert.AreEqual("0.0.0.1", actual);
            
        }


    }
}