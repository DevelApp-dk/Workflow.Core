using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractBehaviors
{
    /// <summary>
    /// Get data to sender sends lookup data to sender
    /// </summary>
    public abstract class GetDataToSenderSagaStepBehavior:SagaStepBehavior
    {
        /// <summary>
        /// Returns the behaviorType
        /// </summary>
        public override SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.GetDataToSender;
            }
        }
    }
}
