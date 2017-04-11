using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace Specflow.DSL
{
    [Binding]
    public sealed class ExampleSteps
    {
        [When(@"entered int (.*)")]
        public void GivenEnteredInt(int p0)
        {
           
        }

        [Then(@"verify int (.*) equals (.*)")]
        public void ThenVerifyIntEquals(int p0, int p1)
        {
            Assert.AreEqual(p1, p0);
        }

        [When(@"entered string ""(.*)""")]
        public void GivenEnteredString(string p0)
        {
        }

        [Then(@"verify string ""(.*)"" equals ""(.*)""")]
        public void ThenVerifyStringEquals(string p0, string p1)
        {
            Assert.AreEqual(p1, p0);
        }

        [When(@"entered long string")]
        public void GivenEnteredLongString(string multilineText)
        {
            var target = @"
       ok,
       50";
            Assert.AreEqual(target, multilineText);
        }

        [Then(@"verify string ""(.*)"" matches ""(.*)""")]
        public void ThenVerifyStringMatches(string str2Match, string regex)
        {
            Assert.IsTrue(new Regex(regex).IsMatch(str2Match));
        }

        [Then(@"verify string ""(.*)"" is not defined")]
        public void ThenVerifyStringIsNotDefined(string p0)
        {
            object var;
            Assert.IsFalse(ScenarioContext.Current.TryGetValue(p0, out var));

        }


    }
}
