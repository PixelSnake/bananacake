using BCake.Parser.Errors;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.Parser.Errors
{
    [TestFixture]
    internal class ResultSenseTest
    {
        [Test]
        public void YieldingNoResultsReturnsDefaultValue()
        {
            IEnumerable<Result> testFunc()
            {
                yield return null;
            }

            Assert.AreEqual(testFunc().BoolValue(), ResultSense.Default.DefaultValue);
        }
        
        [Test]
        public void YieldingFalseWithFalseDominatesReturnsFalse()
        {
            IEnumerable<Result> testFunc(bool firstVal, bool secondVal)
            {
                yield return ResultSense.FalseDominates;
                yield return new ResultValue<bool>(firstVal);
                yield return new ResultValue<bool>(secondVal);
            }

            Assert.AreEqual(testFunc(true, false).BoolValue(), false);
            Assert.AreEqual(testFunc(false, true).BoolValue(), false);
            Assert.AreEqual(testFunc(false, false).BoolValue(), false);
            Assert.AreEqual(testFunc(true, true).BoolValue(), true);
        }

        [Test]
        public void YieldingTrueWithTrueDominatesReturnsTrue()
        {
            IEnumerable<Result> testFunc(bool firstVal, bool secondVal)
            {
                yield return ResultSense.TrueDominates;
                yield return new ResultValue<bool>(firstVal);
                yield return new ResultValue<bool>(secondVal);
            }

            Assert.AreEqual(testFunc(true, false).BoolValue(), true);
            Assert.AreEqual(testFunc(false, true).BoolValue(), true);
            Assert.AreEqual(testFunc(false, false).BoolValue(), false);
            Assert.AreEqual(testFunc(true, true).BoolValue(), true);
        }

        [Test]
        public void FirstDominanceReturnsFirstValue()
        {
            IEnumerable<Result> testFunc(bool firstVal)
            {
                yield return ResultSense.First;
                yield return new ResultValue<bool>(firstVal);
                yield return new ResultValue<bool>(!firstVal);
            }

            Assert.AreEqual(testFunc(true).BoolValue(), true);
            Assert.AreEqual(testFunc(false).BoolValue(), false);
        }

        [Test]
        public void LastDominanceReturnsLastValue()
        {
            IEnumerable<Result> testFunc(bool firstVal)
            {
                yield return ResultSense.Last;
                yield return new ResultValue<bool>(firstVal);
                yield return new ResultValue<bool>(!firstVal);
            }

            Assert.AreEqual(testFunc(true).BoolValue(), false);
            Assert.AreEqual(testFunc(false).BoolValue(), true);
        }
    }
}
