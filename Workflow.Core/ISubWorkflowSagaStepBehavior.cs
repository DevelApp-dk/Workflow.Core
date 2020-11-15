using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// SubWorkflow progress the SagaStep throught the SagaStepState from Initiate via WaitForSubWorkflow to Evaluate
    /// </summary>
    public interface ISubWorkflowSagaStepBehavior:ISagaStepBehavior
    {
        /// <summary>
        /// SagaStep is waiting for subworkflows to finish. Will process finish messages from subworkflows and will progress when all subworkflows have finished
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool BeforeWaitForSubWorkflow(ISagaStep sagaStep);
    }
}
