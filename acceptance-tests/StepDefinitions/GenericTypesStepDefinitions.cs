using System;
using BCake.Parser;
using BcakeAcceptanceTests.Support.Context;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BcakeAcceptanceTests.StepDefinitions
{
    [Binding]
    public class GenericTypesStepDefinitions
    {
        private ParserContext _parserContext;

        public GenericTypesStepDefinitions(ParserContext parserContext)
        {
            _parserContext = parserContext;
        }
    }
}
