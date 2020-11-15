using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Automatic progress the SagaStep throught the SagaStepState from Initiate to Evaluate
    /// </summary>
    public interface IAutomaticSagaStepBehavior:ISagaStepBehavior
    {
    }
}
