using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractBehaviors

{
    /// <summary>
    /// Get data to use for later execution of a message for something
    /// </summary>
    public abstract class GetDataToScheduleSagaStepBehavior:SagaStepBehavior
    {
        /// <summary>
        /// Returns the behaviorType
        /// </summary>
        public override SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.GetDataToSchedule;
            }
        }
    }
}
