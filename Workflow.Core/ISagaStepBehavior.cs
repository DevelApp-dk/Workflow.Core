using DevelApp.RuntimePluggableClassFactory.Interface;
using DevelApp.Workflow.Core.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Defines the topmost interface on StateBehavior which is the executable part of a Workflow. This should not be implemented directly
    /// </summary>
    public interface ISagaStepBehavior:IPluginClass
    {
        /// <summary>
        /// Sets the behavior configuration
        /// </summary>
        /// <param name="behaviorConfiguration"></param>
        /// <returns></returns>
        bool SetBehaviorConfiguration(JsonValue behaviorConfiguration);

        /// <summary>
        /// Defines the specific state behavior used for casting
        /// </summary>
        SagaStepBehaviorType BehaviorType { get; }

        /// <summary>
        /// Actual execution of behavior
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        IWorkflowMessage Execute(ISagaStep sagaStep, IWorkflowMessage workflowMessage);

        /// <summary>
        /// ModuleKey for the owning module
        /// </summary>
        KeyString ModuleKey { get; }

        /// <summary>
        /// ModuleKey for the owning workflow if any
        /// </summary>
        KeyString WorkflowKey { get; }
    }
}
