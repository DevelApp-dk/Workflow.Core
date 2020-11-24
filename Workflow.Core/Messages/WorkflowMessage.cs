using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Exceptions;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Core.Messages
{
    /// <summary>
    /// Used as envelope for many dynamic message types used in Workflow
    /// </summary>
    public class WorkflowMessage: IWorkflowMessage
    {
        public static string REPLY = "Reply";
        public static string TIMEOUT = "Timeout";

        public WorkflowMessage(string messageTypeName, JsonDataString data, ActorPath replyTo = null, Guid transactionId = default, Guid transactionGroupId = default)
        {
            if (transactionId == default)
            {
                TransactionId = Guid.NewGuid();
            }
            else
            {
                TransactionId = transactionId;
            }
            if (transactionGroupId == default)
            {
                TransactionGroupId = Guid.NewGuid();
            }
            else
            {
                TransactionGroupId = transactionGroupId;
            }
            MessageTypeName = messageTypeName;
            Data = data;
            ReplyTo = replyTo;
            MessageCreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// The transaction id of the message unique for the interaction
        /// </summary>
        public Guid TransactionId { get; }

        /// <summary>
        /// The transaction group id of the message
        /// </summary>
        public Guid TransactionGroupId { get; }

        /// <summary>
        /// MessageTypeName is used to distinguish what the message is about
        /// </summary>
        public string MessageTypeName { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime MessageCreationTime { get; }

        /// <summary>
        /// Dynamic data for Message
        /// </summary>
        public JsonDataString Data { get; }

        /// <summary>
        /// Can contain the expected replyTo of the message if a reply is expected
        /// </summary>
        public ActorPath ReplyTo { get; }

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
                return false;
            }
        }

        /// <summary>
        /// Returns a instance of a failed message
        /// </summary>
        public WorkflowSucceededMessage GetWorkflowSucceededMessage(ActorPath replyingReceiver)
        {
            return new WorkflowSucceededMessage(this, replyingReceiver);
        }

        /// <summary>
        /// Returns a instance of a failed message
        /// </summary>
        public WorkflowFailedMessage GetWorkflowFailedMessage(ActorPath reportingActor, string errorMessage = null, Exception ex = null)
        {
            return new WorkflowFailedMessage(this, reportingActor, ex, errorMessage);
        }

        /// <summary>
        /// Returns a instance of an unhandled message
        /// </summary>
        public WorkflowUnhandledMessage GetWorkflowUnhandledMessage(string unhandledExplanation, ActorPath replyingReceiver)
        {
            return new WorkflowUnhandledMessage(this, unhandledExplanation, replyingReceiver);
        }
    }

    /// <summary>
    /// Envelope for failed  message sent to replyto actor
    /// </summary>
    public class WorkflowFailedMessage : IWorkflowMessage
    {
        internal WorkflowFailedMessage(WorkflowMessage workflowMessage, ActorPath reportingActor, Exception ex, string errorMessage)
        {
            if (workflowMessage.ReplyTo == null)
            {
                throw new WorkflowRuntimeException($"Trying to reply to workflowMessage {workflowMessage.MessageTypeName} Id {workflowMessage.TransactionId} without replyTo is set");
            }
            TransactionId = workflowMessage.TransactionId;
            TransactionGroupId = workflowMessage.TransactionGroupId;
            MessageTypeName = workflowMessage.MessageTypeName + WorkflowMessage.REPLY;
            Data = workflowMessage.Data;
            OriginalMessageCreationTime = workflowMessage.MessageCreationTime;
            MessageCreationTime = DateTime.UtcNow;
            ReportingActor = reportingActor;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        /// <summary>
        /// The transaction id of the message unique for the interaction
        /// </summary>
        public Guid TransactionId { get; }

        /// <summary>
        /// The transaction group id of the message
        /// </summary>
        public Guid TransactionGroupId { get; }

        /// <summary>
        /// The reported error message if reported
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// The thrown exception if reported
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// MessageTypeName is used to distinguish what the message is about
        /// </summary>
        public string MessageTypeName { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime MessageCreationTime { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime OriginalMessageCreationTime { get; }

        /// <summary>
        /// Dynamic data for Message
        /// </summary>
        public JsonDataString Data { get; }

        /// <summary>
        /// Can contain the reporting actor of the error
        /// </summary>
        public ActorPath ReportingActor { get; }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsReply
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsTimeout
        {
            get
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Envelope for succeeded messages sent to replyto actor
    /// </summary>
    public class WorkflowSucceededMessage : IWorkflowMessage
    {
        internal WorkflowSucceededMessage(WorkflowMessage workflowMessage, ActorPath replyingReceiver)
        {
            if (workflowMessage.ReplyTo == null)
            {
                throw new WorkflowRuntimeException($"Trying to reply to workflowMessage {workflowMessage.MessageTypeName} Id {workflowMessage.TransactionId} without replyTo is set");
            }
            TransactionId = workflowMessage.TransactionId;
            TransactionGroupId = workflowMessage.TransactionGroupId;
            MessageTypeName = workflowMessage.MessageTypeName + WorkflowMessage.REPLY;
            OriginalMessageCreationTime = workflowMessage.MessageCreationTime;
            MessageCreationTime = DateTime.UtcNow;
            ReplyingReceiver = replyingReceiver;
        }

        /// <summary>
        /// The transaction id of the message unique for the interaction
        /// </summary>
        public Guid TransactionId { get; }

        /// <summary>
        /// The transaction group id of the message
        /// </summary>
        public Guid TransactionGroupId { get; }

        /// <summary>
        /// MessageTypeName is used to distinguish what the message is about
        /// </summary>
        public string MessageTypeName { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime MessageCreationTime { get; }

        /// <summary>
        /// Time of original message creation in UTC time
        /// </summary>
        public DateTime OriginalMessageCreationTime { get; }

        /// <summary>
        /// Can contain the replying actor
        /// </summary>
        public ActorPath ReplyingReceiver { get; }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsReply
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsTimeout
        {
            get
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Envelope for message unhandled by recipient for some reason sent to replyto actor
    /// </summary>
    public class WorkflowUnhandledMessage: IWorkflowMessage
    {
        internal WorkflowUnhandledMessage(WorkflowMessage workflowMessage, string unhandledExplanation, ActorPath replyingReceiver)
        {
            if(workflowMessage.ReplyTo == null)
            {
                throw new WorkflowRuntimeException($"Trying to reply to workflowMessage {workflowMessage.MessageTypeName} Id {workflowMessage.TransactionId} without replyTo is set");
            }
            TransactionId = workflowMessage.TransactionId;
            TransactionGroupId = workflowMessage.TransactionGroupId;
            UnhandledExplanation = unhandledExplanation;
            MessageTypeName = workflowMessage.MessageTypeName + WorkflowMessage.REPLY;
            Data = workflowMessage.Data;
            ReplyingReceiver = replyingReceiver;
            OriginalMessageCreationTime = workflowMessage.MessageCreationTime;
            MessageCreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// The transaction id of the message unique for the interaction
        /// </summary>
        public Guid TransactionId { get; }

        /// <summary>
        /// The transaction group id of the message
        /// </summary>
        public Guid TransactionGroupId { get; }

        /// <summary>
        /// MessageTypeName is used to distinguish what the message is about
        /// </summary>
        public string MessageTypeName { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime MessageCreationTime { get; }

        /// <summary>
        /// Time of original message creation in UTC time
        /// </summary>
        public DateTime OriginalMessageCreationTime { get; }

        /// <summary>
        /// Dynamic data for Message
        /// </summary>
        public JsonDataString Data { get; }

        /// <summary>
        /// Can contain the replying receiver of the message
        /// </summary>
        public ActorPath ReplyingReceiver { get; }

        /// <summary>
        /// Explanation of why this was unhandled
        /// </summary>
        public string UnhandledExplanation { get; }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsReply
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        public bool IsTimeout
        {
            get
            {
                return false;
            }
        }
    }
}
