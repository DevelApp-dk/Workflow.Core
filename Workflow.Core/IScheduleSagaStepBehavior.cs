using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Schedule progress the SagaStep throught the SagaStepState from Initiate via WaitForSchedule to Evaluate. Schedule is persisted
    /// </summary>
    public interface IScheduleSagaStepBehavior:ISagaStepBehavior
    {
        /// <summary>
        /// SagaStep is waiting for a sheduled timer before progressing. Will go out of memory just after scheduling as the schedule might be in long time
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool BeforeWaitForState(ISagaStep sagaStep);
    }
}
