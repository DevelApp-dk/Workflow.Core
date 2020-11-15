using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// UserInteraction progress the SagaStep throught the SagaStepState from Initiate via WaitForUserInteraction to Evaluate
    /// </summary>
    public interface IUserInteractionSagaStepBehavior:ISagaStepBehavior
    {
        /// <summary>
        /// SagaStep is waiting for user interaction that is data. Will proces messages for intermediate data and save that on reception
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool BeforeWaitForUserInteraction(ISagaStep sagaStep);
    }
}
