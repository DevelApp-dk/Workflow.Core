using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Convenience implementation of IAutomaticStateBehavior
    /// </summary>
    public abstract class AbstractAutomaticSagaBehavior : AbstractSagaStepBehavior, IAutomaticSagaStepBehavior
    {
        public SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.Automatic;
            }
        }
    }
}
