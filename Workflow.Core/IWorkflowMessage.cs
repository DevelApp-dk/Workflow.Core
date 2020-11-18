using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Used as envelope for many dynamic message types used in Workflow
    /// </summary>
    public interface IWorkflowMessage
    {
        /// <summary>
        /// MessageTypeName is used to distinguish what the message is about
        /// </summary>
        string MessageTypeName { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        DateTime MessageCreationTime { get; }

        /// <summary>
        /// Type of dynamic data for Message
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// Can contain the original sender of the message
        /// </summary>
        ActorPath OriginalSender { get; }
    }
}
