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
        /// Automatic progress the SagaStep throught the SagaStepState from Initiate to Evaluate
        /// </summary>
        Automatic = 1,
        /// <summary>
        /// UserInteraction progress the SagaStep throught the SagaStepState from Initiate via WaitForUserInteraction to Evaluate
        /// </summary>
        UserInteraction = 2,
        /// <summary>
        /// SubWorkflow progress the SagaStep throught the SagaStepState from Initiate via WaitForSubWorkflow to Evaluate
        /// </summary>
        SubWorkflow = 3,
        /// <summary>
        /// Schedule progress the SagaStep throught the SagaStepState from Initiate via WaitForSchedule to Evaluate. Schedule is persisted
        /// </summary>
        Schedule = 4
    }

    public enum SagaStepStateType
    {
        /// <summary>
        /// Type is unknown which is an uninitialized or an error state for type
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// SagaStep is gathering data for the SagaStep
        /// </summary>
        Initiate = 1,
        /// <summary>
        /// SagaStep is waiting for user interaction that is data. Will proces messages for intermediate data and save that on reception
        /// </summary>
        WaitForUserInteraction = 2,
        /// <summary>
        /// SagaStep is waiting for subworkflows to finish. Will process finish messages from subworkflows and will progress when all subworkflows have finished
        /// </summary>
        WaitForSubWorkflow = 3,
        /// <summary>
        /// SagaStep is waiting for a sheduled timer before progressing. Will go out of memory just after scheduling as the schedule might be in long time
        /// </summary>
        WaitForSchedule = 4,
        /// <summary>
        /// Evaluates the gathered data and decides on an outcome 
        /// </summary>
        Evaluate = 5
    }
}
