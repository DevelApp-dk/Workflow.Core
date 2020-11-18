using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Starts subworkflows that are automatically used for progressing when finished.
    /// </summary>
    public abstract class StartSubWorkflowsSagaStepBehavior:SagaStepBehavior
    {
        /// <summary>
        /// Returns the behaviorType
        /// </summary>
        public override SagaStepBehaviorType BehaviorType
        {
            get
            {
                return SagaStepBehaviorType.StartSubWorkflows;
            }
        }
    }
}
