using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractBehaviors
{
    /// <summary>
    /// Behavior evaluates the data and the next step
    /// </summary>
    public abstract class EvaluateNextStepSagaStepBehavior:SagaStepBehavior
    {
        /// <summary>
        /// Returns the behaviorType
        /// </summary>
        public override SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.EvaluateNextStep;
            }
        }
    }
}
