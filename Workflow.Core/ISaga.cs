using DevelApp.Workflow.Core.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    public interface ISaga
    {
        /// <summary>
        /// Returns the Saga version
        /// </summary>
        /// <returns></returns>
        VersionString SagaVersion { get; }

        /// <summary>
        /// Returns the first SagaStep of the Saga
        /// </summary>
        /// <returns></returns>
        ISagaStep FirstSagaStep { get; }

        /// <summary>
        /// Returns the latest SagaStep of the Saga
        /// </summary>
        /// <returns></returns>
        ISagaStep LatestSagaStep { get; }

        /// <summary>
        /// Possibility to check if SagaStep data is valid
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool DataIsValid(ISagaStep sagaStep);

        /// <summary>
        /// Possibility to check if SagaStep DataJsonSchema is valid before progressing
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool DataJsonSchemaIsValid(ISagaStep sagaStep);

        /// <summary>
        /// Possibility to check validation details of SagaStep data
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        SchemaValidationResults CheckData(ISagaStep sagaStep);

        /// <summary>
        /// Possibility to check validation details of SagaStep DataJsonSchema. Returns null if DataJsonSchema is not found
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        MetaSchemaValidationResults CheckDataJsonSchema(ISagaStep sagaStep);
    }
}
