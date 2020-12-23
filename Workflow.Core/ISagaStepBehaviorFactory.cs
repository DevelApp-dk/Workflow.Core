using DevelApp.Utility.Model;
using Manatee.Json;

namespace DevelApp.Workflow.Core
{
    public interface ISagaStepBehaviorFactory
    {
        /// <summary>
        /// Returns an instance of the behavior
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="behaviorName"></param>
        /// <param name="version"></param>
        /// <param name="behaviorConfiguration"></param>
        /// <returns></returns>
        ISagaStepBehavior GetSagaStepBehavior(KeyString moduleName, KeyString behaviorName, SemanticVersionNumber version, JsonValue behaviorConfiguration, KeyString workflowName = null);
    }
}
