using DevelApp.JsonSchemaBuilder;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Core.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    public interface IJsonSchemaDefinitionFactory
    {
        /// <summary>
        /// Returns a jsonSchemaDefinition from plugin
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="jsonSchemaName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        IJsonSchemaDefinition GetJsonSchemaDefinition(KeyString moduleName, KeyString jsonSchemaName, SemanticVersionNumber version);
    }
}
