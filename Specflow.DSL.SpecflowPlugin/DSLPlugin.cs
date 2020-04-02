using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.UnitTestProvider;

[assembly: RuntimePlugin(typeof(Specflow.DSL.DSLPlugin))]

namespace Specflow.DSL
{
    public class DSLPlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters,
            UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.CustomizeTestThreadDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<DSLTestRunner, ITestRunner>();
                args.ObjectContainer.RegisterTypeAs<ParameterTransform, IParameterTransform>();
            };
        }
    }
}