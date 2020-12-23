using DevelApp.RuntimePluggableClassFactory.Interface;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractBehaviors
{
    /// <summary>
    /// Placement of utility functions for all SagaStepBehaviors. Do not inherit from this
    /// </summary>
    public abstract class SagaStepBehavior:ISagaStepBehavior
    {
        /// <summary>
        /// ModuleKey for the owning module
        /// </summary>
        public abstract KeyString ModuleKey { get; }

        /// <summary>
        /// WorkflowKey for the owning workflow if any
        /// </summary>
        public abstract KeyString WorkflowKey { get; }

        /// <summary>
        /// ModuleKey for the owning workflow if any
        /// </summary>
        public KeyString BehaviorKey 
        { 
            get
            {
                return Name.ToString();
            }
        }

        /// <summary>
        /// Returns the specific name of the sagastep behavior without Behavior or SagaStepBehavior as they are implied
        /// </summary>
        public IdentifierString Name
        {
            get
            {
                return GetType().Name.Replace("SagaStepBehavior", "").Replace("Behavior", "");
            }
        }

        public NamespaceString Module
        {
            get
            {
                return ModuleKey + (string.IsNullOrWhiteSpace(WorkflowKey) ? "" : "." + WorkflowKey);
            }
        }


        /// <summary>
        /// Description of the sagastep behavior
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Returns the version number of the sagastep behavior
        /// </summary>
        public abstract SemanticVersionNumber Version { get; }

        /// <summary>
        /// Returns the JsonSchema for the configuration
        /// </summary>
        /// <returns></returns>
        public abstract JsonSchema GetConfigurationJsonSchema();

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
        /// Executes the behavior
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <param name="workflowMessage"></param>
        /// <returns></returns>
        public abstract IWorkflowMessage Execute(ISagaStep sagaStep, WorkflowMessage workflowMessage);

        /// <summary>
        /// The validated behaviorfor the SagaStepBehavior
        /// </summary>
        protected JsonValue BehaviorConfiguration { get; private set; }

        /// <summary>
        /// Returns the behaviorType
        /// </summary>
        public abstract SagaStepBehaviorType BehaviorType { get; }
    }
}
