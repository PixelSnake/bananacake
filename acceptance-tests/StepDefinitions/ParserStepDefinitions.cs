using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcakeAcceptanceTests.Support.Context;
using NUnit.Framework;

namespace BcakeAcceptanceTests.StepDefinitions
{
    [Binding]
    public class ParserStepDefinitions
    {
        private ParserContext _parserContext;

        public ParserStepDefinitions(ParserContext parserContext)
        {
            _parserContext = parserContext;
        }

        [Then(@"an error is returned")]
        public void ThenAnErrorIsReturned()
        {
            Assert.IsTrue(_parserContext.HasErrors);
        }

        [Then(@"the error contains ""([^""]*)""")]
        public void TheErrorContains(string error)
        {
            Assert.IsNotNull(_parserContext.ErrorMessage);
            Assert.IsTrue(_parserContext.ErrorMessage?.Contains(error));
        }

        [Then(@"there are no errors")]
        public void ThenThereAreNoErrors()
        {
            Assert.IsFalse(_parserContext.HasErrors);
        }
    }
}
