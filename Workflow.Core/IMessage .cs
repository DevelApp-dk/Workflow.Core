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
        Guid TransactionId { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        DateTime MessageCreationTime { get; }

        /// <summary>
        /// The transaction group id of the message
        /// </summary>
        Guid TransactionGroupId { get; }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        bool IsReply { get; }

        /// <summary>
        /// Is the message a timeout
        /// </summary>
        bool IsTimeout { get; }
    }
}
