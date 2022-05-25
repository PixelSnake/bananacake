using BcakeAcceptanceTests.Support.Context;
using System;
using TechTalk.SpecFlow;

namespace BcakeAcceptanceTests.StepDefinitions
{
    [Binding]
    public class CodeStepDefinitions
    {
        private ParserContext _parserContext;

        public CodeStepDefinitions(ParserContext parserContext)
        {
            _parserContext = parserContext;
        }

        [Given(@"the following ([a-z]+) is defined:")]
        public void GivenTheFollowingClassIsDefined(string type, string multilineText)
        {
            _parserContext.AddCode(multilineText);
        }

        [When(@"the code is compiled")]
        public void WhenTheCodeIsCompiled()
        {
            _parserContext.Run();
        }

        [Given(@"the main function contains the following code:")]
        public void GivenTheMainFunctionContainsTheFollowingCode(string multilineText)
        {
            _parserContext.AddCode(@"
                int main() {
                    " + multilineText + @"

                    return 0;
                }
            ");
        }

        [Given(@"the main function is empty")]
        public void GivenTheMainFunctionIsEmpty()
        {
            _parserContext.AddCode(@"
                int main() {
                    return 0;
                }
            ");
        }
    }
}
