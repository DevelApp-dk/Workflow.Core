using DevelApp.RuntimePluggableClassFactory.Interface;
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
        /// SagaStep is gathering data for the SagaStep
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool Initiate(ISagaStep sagaStep);

        /// <summary>
        /// Evaluates the gathered data and decides on an outcome 
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool Evaluate(ISagaStep sagaStep);

        /// <summary>
        /// Called on errors in one of the behavior steps returns false if error could not be resolved
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool Error(ISagaStep sagaStep);
    }
}
