using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Reply of unhandled message envelope
    /// </summary>
    public interface IWorkflowUnhandledMessage:IWorkflowMessage
    {
        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        DateTime OriginalMessageCreationTime { get; }

        /// <summary>
        /// Can contain the explanation why the message was unhandled
        /// </summary>
        string UnhandledExplanation { get; }

        /// <summary>
        /// Can contain the replying receiver of the message
        /// </summary>
        ActorPath ReplyingReceiver { get; }
    }
}
