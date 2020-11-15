using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    public interface ISagaStep
    {
        /// <summary>
        /// Returns the owning Saga
        /// </summary>
        /// <returns></returns>
        ISaga Saga { get; }

        /// <summary>
        /// Returns all the previous SagaSteps in the order previous first
        /// </summary>
        /// <returns></returns>
        IEnumerable<ISagaStep> AllPreviousSagaSteps { get; }

        /// <summary>
        /// Returns the previous SagaStep
        /// </summary>
        /// <returns></returns>
        ISagaStep PreviousSagaStep { get; }

        /// <summary>
        /// Returns the data of the current SagaStep
        /// </summary>
        /// <returns></returns>
        JsonValue Data { get; set; }

        /// <summary>
        /// The JsonSchema for the data of the current SagaStep
        /// </summary>
        JsonSchema DataJsonSchema { get; set; }

        /// <summary>
        /// Returns the configuration for the SagaStep
        /// </summary>
        /// <returns></returns>
        JsonValue Configuration { get; }
    }
}
