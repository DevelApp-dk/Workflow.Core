using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Messages that can handle grouping
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The transaction id of the message unique for the interaction
        /// </summary>
        TransactionId TransactionId { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        DateTime MessageCreationTime { get; }

        /// <summary>
        /// The transaction group id of the message
        /// </summary>
        TransactionGroupId TransactionGroupId { get; }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        bool IsReply { get; }

        /// <summary>
        /// Is the message a timeout
        /// </summary>
        bool IsTimeout { get; }

        /// <summary>
        /// Can contain the expected replyTo of the message if a reply is expected
        /// </summary>
        ActorPath ReplyTo { get; }
    }
}
