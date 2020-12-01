using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.Messages
{
    /// <summary>
    /// Envelope for timeout of group of messages sent to replyto actor
    /// </summary>
    public class GroupTimedOutMessage : IMessage
    {
        public GroupTimedOutMessage(Guid transactionGroupId)
        {
            TransactionId = Guid.NewGuid(); ;
            TransactionGroupId = transactionGroupId;
            MessageCreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// The transaction id of the message unique for the interaction
        /// </summary>
        public TransactionId TransactionId { get; }

        /// <summary>
        /// The transaction group id of the message
        /// </summary>
        public TransactionGroupId TransactionGroupId { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime MessageCreationTime { get; }

        /// <summary>
        /// Can contain the expected replyTo of the message if a reply is expected
        /// </summary>
        public ActorPath ReplyTo { get { return null; } }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsReply
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsTimeout
        {
            get
            {
                return true;
            }
        }
    }
}
