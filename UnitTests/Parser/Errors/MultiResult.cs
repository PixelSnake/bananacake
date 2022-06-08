using BCake.Parser;
using BCake.Parser.Errors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Parser.Errors
{
    [TestFixture]
    public class MultiResult
    {
        [Test]
        public void UnpackingWorksCorrectly()
        {
            IEnumerable<Result> inner2()
            {
                yield return ResultSense.FalseDominates;
                yield return new Error("Test", Token.Anonymous(""));
                yield return Result.False;
            }

            IEnumerable<Result> inner1()
            {
                yield return ResultSense.FalseDominates;
                yield return inner2().Pack();
            }

            Assert.AreEqual(inner1().HasErrors(), true);
        }
    }
}
