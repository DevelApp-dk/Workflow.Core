using DevelApp.Utility.Model;
using DevelApp.Workflow.Core.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    public interface ISagaPayload
    {
        /// <summary>
        /// Get the dataOwnerKey from the ActorPath
        /// </summary>
        KeyString GetDataOwnerKey { get; }

        /// <summary>
        /// Get the moduleKey from the ActorPath
        /// </summary>
        KeyString GetModuleKey { get; }

        /// <summary>
        /// Get the workflowKey from the ActorPath
        /// </summary>
        KeyString GetWorkflowKey { get; }

        /// <summary>
        /// Can contain the nodeKey of expected node in which the saga is in. If containing data and not in the expected node the message is discarded
        /// </summary>
        KeyString ExpectedNodeKey { get; }

        /// <summary>
        /// Can contain the edgeKey to progress via
        /// </summary>
        KeyString ProgressViaEdgeKey { get; }

        /// <summary>
        /// Get the contained data
        /// </summary>
        JsonValue Data { get; }
    }
}
