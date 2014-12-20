using System;
using Octono.Xrm.Tasks;
using Xunit;

namespace Octono.Xrm.Tests
{
    
    public class SolutionVersionFormatterTests
    {
        [Fact]
        public void CanIncrementRevisionNumberFromValidInput()
        {
            var formatter = new SolutionVersionFormatter();
            string actual = formatter.Increment("0.0.0.0");
            Assert.Equal("0.0.0.1",actual);
        }

        [Fact]
        public void ThrowsFormatExceptionWhenVersionNumberInvalid()
        {
            var formatter = new SolutionVersionFormatter();
            Assert.Throws<FormatException>(() => formatter.Increment("ABC"));
        }

        [Fact]
        public void SupportsIncrementMajorMinorVersionNumber()
        {
            var formatter = new SolutionVersionFormatter();
            string actual = formatter.Increment("1.0");
            Assert.Equal("1.1", actual);
        }

        [Fact]
        public void CreatesNewSemanticVersionNumberWhenInputIsEmptyString()
        {
            var formatter = new SolutionVersionFormatter();
            string actual = formatter.Increment("");
            Assert.Equal("0.0.1", actual);
            
        }


    }
}