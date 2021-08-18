using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace Specflow.DSL
{
    class DSLTestRunner : ITestRunner
    {

        ITestRunner _TestRunner;
        IParameterTransform _Transform;

        public DSLTestRunner(ITestExecutionEngine executionEngine, IParameterTransform tranform)
        {
            _TestRunner = new TestRunner(executionEngine);
            _Transform = tranform;
        }

        public string Transform(string obj)
        {
            return _Transform.Transform(obj, ScenarioContext);
        }

        public Table Transform(Table table)
        {
            if (table == null) return table;

            foreach (var row in table.Rows)
            {
                foreach (var k in row.Keys)
                    row[k] = _Transform.Transform(row[k], ScenarioContext);
            }
            return table;
        }

        public FeatureContext FeatureContext => _TestRunner.FeatureContext;

        public ScenarioContext ScenarioContext => _TestRunner.ScenarioContext;

        public int ThreadId => _TestRunner.ThreadId;

        public void And(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            _TestRunner.And(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public void But(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            _TestRunner.But(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public void CollectScenarioErrors()
        {
            _TestRunner.CollectScenarioErrors();
        }

        public void Given(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            _TestRunner.Given(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
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
            _TestRunner.Then(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public void When(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            _TestRunner.When(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public void OnScenarioInitialize(ScenarioInfo scenarioInfo)
        {
            _TestRunner.OnScenarioInitialize(scenarioInfo);
        }

        public void OnScenarioStart()
        {
            _TestRunner.OnScenarioStart();
        }

        public void SkipScenario()
        {
            _TestRunner.SkipScenario();
        }
    }
}