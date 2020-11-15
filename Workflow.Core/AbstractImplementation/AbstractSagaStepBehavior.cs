using DevelApp.RuntimePluggableClassFactory.Interface;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Placement of utility functions for all SagaStepBehaviors
    /// </summary>
    public abstract class AbstractSagaStepBehavior
    {
        /// <summary>
        /// Returns the specific name of the sagastep behavior
        /// </summary>
        public string Name
        {
            get
            {
                return GetType().FullName;
            }
        }

        /// <summary>
        /// Description of the sagastep behavior
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Returns the version number of the sagastep behavior
        /// </summary>
        public abstract int Version { get; }

        /// <summary>
        /// SagaStep is gathering data for the SagaStep
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool Initiate(ISagaStep sagaStep);

        /// <summary>
        /// Evaluates the gathered data and decides on an outcome 
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool Evaluate(ISagaStep sagaStep);

        /// <summary>
        /// Returns the JsonSchema for the configuration
        /// </summary>
        /// <returns></returns>
        public abstract JsonSchema GetConfigurationJsonSchema();

        /// <summary>
        /// Called on errors in one of the behavior steps returns false if error could not be resolved
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>

        public abstract bool Error(ISagaStep sagaStep);

        /// <summary>
        /// Sets the behavior configuration
        /// </summary>
        /// <param name="behaviorConfiguration"></param>
        /// <returns></returns>
        public bool SetBehaviorConfiguration(JsonValue behaviorConfiguration)
        {
            SchemaValidationResults schemaValidationResults = GetConfigurationJsonSchema().Validate(behaviorConfiguration);
            if (schemaValidationResults.IsValid)
            {
                BehaviorConfiguration = behaviorConfiguration;
            }
            return schemaValidationResults.IsValid;
        }

        /// <summary>
        /// The validated behaviorfor the SagaStepBehavior
        /// </summary>
        protected JsonValue BehaviorConfiguration { get; private set; }
    }
}
