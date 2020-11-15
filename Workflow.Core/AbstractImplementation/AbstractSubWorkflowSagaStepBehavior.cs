using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Convenience implementation of ISubWorkflowStateBehavior
    /// </summary>
    public abstract class AbstractSubWorkflowSagaStepBehavior : AbstractSagaStepBehavior, ISubWorkflowSagaStepBehavior
    {
        public SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.SubWorkflow;
            }
        }

        /// <summary>
        /// SagaStep is waiting for subworkflows to finish. Will process finish messages from subworkflows and will progress when all subworkflows have finished
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool BeforeWaitForSubWorkflow(ISagaStep sagaStep);
    }
}
