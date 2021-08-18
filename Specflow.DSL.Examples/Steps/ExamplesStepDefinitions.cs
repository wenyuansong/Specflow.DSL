using Specflow.DSL;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace Examples.Steps
{
    [Binding]
    public sealed class ExamplesStepDefinitions
    {
        ScenarioContext _context;

        public ExamplesStepDefinitions(ScenarioContext context)
        {
            _context = context;
        }

        [When(@"entered int (.*)")]
        public void GivenEnteredInt(int p0)
        {

        }

        [Then(@"verify int (.*) equals (.*)")]
        public void ThenVerifyIntEquals(int p0, int p1)
        {
            p1.Should().Be(p0);
        }

        [When(@"entered string ""(.*)""")]
        public void GivenEnteredString(string p0)
        {
        }

        [Given(@"I have a cutomerise pattern mapping ""(.*)"" to ""(.*)""")]
        public void GivenIHaveACutomerisePatternMappingTo(string keyword, string value)
        {
            ((IParameterTransform)
                (_context.GetBindingInstance(typeof(IParameterTransform))))
            .addTransformer(s => s.ToLower() == keyword.ToLower() ? value : s);
        }

        [Given(@"I have a cutomerise pattern to support calculation")]
        public void GivenIHaveACutomerisePatternToSupportCalculation()
        {
            ((IParameterTransform)
                (_context.GetBindingInstance(typeof(IParameterTransform))))
            .addTransformer(s =>
            {
                var m = Regex.Match(s, "([0-9]+)(\\+|\\-|\\*|\\/)([0-9]+)");
                if (m.Success)
                {
                    switch (m.Groups[2].Value)
                    {
                        case "+":
                            return (int.Parse(m.Groups[1].Value) + int.Parse(m.Groups[3].Value)).ToString();
                        case "-":
                            return (int.Parse(m.Groups[1].Value) - int.Parse(m.Groups[3].Value)).ToString();
                        case "*":
                            return (int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[3].Value)).ToString();
                        case "/":
                            return (int.Parse(m.Groups[1].Value) / int.Parse(m.Groups[3].Value)).ToString();
                        default:
                            return s;
                    }
                }
                return s;
            }
                );
        }



        [Given(@"I have a pattern to transform ""(.*)"" to ""(.*)""")]
        public void GivenIHaveAPatternToTransformTo(string p0, string p1)
        {

        }

        [Then(@"verify string ""(.*)"" equals ""(.*)""")]
        public void ThenVerifyStringEquals(string p0, string p1)
        {
            p1.Should().Be(p0);
        }

        [When(@"entered long string")]
        public void GivenEnteredLongString(string multilineText)
        {
            var target = @"
       ok,
       50";
            target.Should().Be(multilineText);
        }

        [Then(@"verify string ""(.*)"" matches ""(.*)""")]
        public void ThenVerifyStringMatches(string str2Match, string regex)
        {
            new Regex(regex).IsMatch(str2Match).Should().BeTrue();
        }

        [Then(@"verify string ""(.*)"" is not defined")]
        public void ThenVerifyStringIsNotDefined(string p0)
        {
            object var;
            _context.TryGetValue(p0, out var).Should().BeFalse();

        }

        [When(@"use table with the following details:")]
        public void WhenUseTableWithTheFollowingDetails(Table table)
        {

        }
    }
}
