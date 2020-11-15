using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Convenience implementation of IUserInteractionStateBehavior
    /// </summary>
    public abstract class AbstractUserInteractionSagaStepBehavior : AbstractSagaStepBehavior, IUserInteractionSagaStepBehavior
    {
        public SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.UserInteraction;
            }
        }

        /// <summary>
        /// SagaStep is waiting for user interaction that is data. Will proces messages for intermediate data and save that on reception
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool BeforeWaitForUserInteraction(ISagaStep sagaStep);
    }
}
