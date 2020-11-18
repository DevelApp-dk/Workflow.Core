using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Get data to self. Typically as a part of automatic execution
    /// </summary>
    public abstract class GetDataToSelfSagaStepBehavior:SagaStepBehavior
    {
        /// <summary>
        /// Returns the behaviorType
        /// </summary>
        public override SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.GetDataToSelf;
            }
        }
    }
}
