using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Defines the state behavior in the Workflow Node
    /// </summary>
    public enum SagaStepBehaviorType
    {
        /// <summary>
        /// Type is unknown which is an uninitialized or an error state for type
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Get data to self. Typically as a part of automatic execution
        /// </summary>
        GetDataToSelf = 1,
        /// <summary>
        /// Get data to use for later execution of a message for something
        /// </summary>
        GetDataToSchedule = 2,
        /// <summary>
        /// Get data to sender sends lookup data to sender
        /// </summary>
        GetDataToSender = 3,
        /// <summary>
        /// Starts subworkflows that are automatically used for progressing when finished.
        /// </summary>
        StartSubWorkflows = 4,
        /// <summary>
        /// Behavior evaluates the data and the next step
        /// </summary>
        EvaluateNextStep = 5
    }
}
