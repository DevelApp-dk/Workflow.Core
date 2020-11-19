using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Core.Messages
{
    /// <summary>
    /// Used as envelope for many dynamic message types used in Workflow
    /// </summary>
    public class WorkflowMessage: IWorkflowMessage
    {
        public WorkflowMessage(string messageTypeName, JsonDataString data, ActorPath originalSender = default)
        {
            MessageTypeName = messageTypeName;
            Data = data;
            if (originalSender != default)
            {
                OriginalSender = originalSender;
            }
            MessageCreationTime = DateTime.UtcNow;
        }

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
        /// Can contain the original sender of the message
        /// </summary>
        public ActorPath OriginalSender { get; }

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

    public class WorkflowFailedMessage: IWorkflowMessage
    {
        internal WorkflowFailedMessage(WorkflowMessage workflowMessage, ActorPath reportingActor, Exception ex, string errorMessage)
        {
            MessageTypeName = workflowMessage.MessageTypeName;
            Data = workflowMessage.Data;
            OriginalSender = workflowMessage.OriginalSender;
            OriginalMessageCreationTime = workflowMessage.MessageCreationTime;
            MessageCreationTime = DateTime.UtcNow;
            ReportingActor = reportingActor;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

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
        /// Can contain the original sender of the message
        /// </summary>
        public ActorPath OriginalSender { get; }
    }

    /// <summary>
    /// Used as envelope for many dynamic message types used in Workflow
    /// </summary>
    public class WorkflowUnhandledMessage: IWorkflowMessage
    {
        internal WorkflowUnhandledMessage(WorkflowMessage workflowMessage, string unhandledExplanation, ActorPath replyingReceiver)
        {
            UnhandledExplanation = unhandledExplanation;
            MessageTypeName = workflowMessage.MessageTypeName;
            Data = workflowMessage.Data;
            ReplyingReceiver = replyingReceiver;
            OriginalSender = workflowMessage.OriginalSender;
            OriginalMessageCreationTime = workflowMessage.MessageCreationTime;
            MessageCreationTime = DateTime.UtcNow;
        }

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
        /// Can contain the replying receiver of the message
        /// </summary>
        public ActorPath ReplyingReceiver { get; }

        /// <summary>
        /// Can contain the original sender of the message
        /// </summary>
        public ActorPath OriginalSender { get; }

        /// <summary>
        /// Explanation of why this was unhandled
        /// </summary>
        public string UnhandledExplanation { get; }
    }
}
