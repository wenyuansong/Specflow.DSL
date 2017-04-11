using BoDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Infrastructure;

namespace Specflow.DSL
{
    class DSLTestRunner : ITestRunner
    {

        ITestRunner _TestRunner;
        IParameterTransform _Transform;

        public DSLTestRunner(ITestExecutionEngine executionEngine)
        {
            _TestRunner = new TestRunner(executionEngine);
            _Transform = new ParameterTransform();
        }

        public FeatureContext FeatureContext => _TestRunner.FeatureContext;

        public ScenarioContext ScenarioContext => _TestRunner.ScenarioContext;

        public int ThreadId => _TestRunner.ThreadId;
       
        public void And(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            _TestRunner.And(_Transform.Transform(text), _Transform.Transform(multilineTextArg), _Transform.Transform(tableArg), keyword);
        }

        public void But(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            _TestRunner.But(_Transform.Transform(text), _Transform.Transform(multilineTextArg), _Transform.Transform(tableArg), keyword);
        }

        public void CollectScenarioErrors()
        {
            _TestRunner.CollectScenarioErrors();
        }

        public void Given(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            _TestRunner.Given(_Transform.Transform(text), _Transform.Transform(multilineTextArg), _Transform.Transform(tableArg), keyword);
        }

        public void InitializeTestRunner(int threadId)
        {
            _TestRunner.InitializeTestRunner(threadId);
        }

        public void OnFeatureEnd()
        {
            _TestRunner.OnFeatureEnd();
        }

        public void OnFeatureStart(FeatureInfo featureInfo)
        {
            _TestRunner.OnFeatureStart(featureInfo);
        }

        public void OnScenarioEnd()
        {
            _TestRunner.OnScenarioEnd();
        }

        public void OnScenarioStart(ScenarioInfo scenarioInfo)
        {
            _TestRunner.OnScenarioStart(scenarioInfo);
        }

        public void OnTestRunEnd()
        {
            _TestRunner.OnTestRunEnd();
        }

        public void OnTestRunStart()
        {
            _TestRunner.OnTestRunStart();
        }

        public void Pending()
        {
            _TestRunner.Pending();
        }

        public void Then(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            var p = new ParameterTransform();
            _TestRunner.Then(_Transform.Transform(text), _Transform.Transform(multilineTextArg), _Transform.Transform(tableArg), keyword);
        }

        public void When(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            //ScenarioContext.Current
            var p = new ParameterTransform();
            
            _TestRunner.When(_Transform.Transform(text), _Transform.Transform(multilineTextArg), _Transform.Transform(tableArg), keyword);
        }
    }
}
