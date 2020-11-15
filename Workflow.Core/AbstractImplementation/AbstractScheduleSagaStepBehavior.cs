using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Convenience implementation of IScheduleStateBehavior
    /// </summary>
    public abstract class AbstractScheduleSagaStepBehavior : AbstractSagaStepBehavior, IScheduleSagaStepBehavior
    {
        public SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.Schedule;
            }
        }

        /// <summary>
        /// SagaStep is waiting for a sheduled timer before progressing. Will go out of memory just after scheduling as the schedule might be in long time
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool BeforeWaitForState(ISagaStep sagaStep);
    }
}
