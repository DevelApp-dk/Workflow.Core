using DevelApp.Workflow.Core.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    public interface ISagaStepBehaviorFactory
    {
        /// <summary>
        /// Returns an instance of the behavior
        /// </summary>
        /// <param name="behaviorName"></param>
        /// <returns></returns>
        ISagaStepBehavior GetSagaStepBehavior(KeyString behaviorName, VersionNumber version, JsonValue behaviorConfiguration);
    }
}
