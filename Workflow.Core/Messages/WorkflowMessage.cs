using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using DevelApp.Workflow.Core;

namespace DevelApp.Workflow.Core.Messages
{
    /// <summary>
    /// Used as envelope for many dynamic message types used in Workflow
    /// </summary>
    public class WorkflowMessage<T> : IWorkflowMessage
    {
        public WorkflowMessage(string messageTypeName, T data, ActorPath originalSender = default)
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
        /// Returns type of generic payload
        /// </summary>
        public Type DataType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Dynamic data for Message
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Can contain the original sender of the message
        /// </summary>
        public ActorPath OriginalSender { get; }

        /// <summary>
        /// Returns a instance of a failed message
        /// </summary>
        public WorkflowFailedMessage<T> GetWorkflowFailedMessage(string errorMessage = null, Exception ex = null)
        {
            return new WorkflowFailedMessage<T>(this, errorMessage, ex);
        }

        /// <summary>
        /// Returns a instance of an unhandled message
        /// </summary>
        public WorkflowUnhandledMessage<T> GetWorkflowUnhandledMessage(string unhandledExplanation, ActorPath replyingReceiver)
        {
            return new WorkflowUnhandledMessage<T>(this, unhandledExplanation, replyingReceiver);
        }
    }

    public class WorkflowFailedMessage<V> :IWorkflowFailedMessage
    {
        internal WorkflowFailedMessage(WorkflowMessage<V> workflowMessage, string errorMessage) : this(sagaMessage, null, errorMessage)
        {
        }

        internal WorkflowFailedMessage(WorkflowMessage<V> workflowMessage, Exception ex errorMessage) : this(sagaMessage, ex, null)
        {
        }

        internal WorkflowFailedMessage(WorkflowMessage<V> workflowMessage, Exception ex, string errorMessage)
        {
            SagaKey = (string)sagaMessage.SagaKey;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public KeyString SagaKey { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }

    /// <summary>
    /// Used as envelope for many dynamic message types used in Workflow
    /// </summary>
    public class WorkflowUnhandledMessage<U> : IWorkflowUnhandledMessage
    {
        internal WorkflowUnhandledMessage(WorkflowMessage<U> workflowMessage, string unhandledExplanation, ActorPath replyingReceiver)
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
        /// Returns type of generic payload
        /// </summary>
        public Type DataType
        {
            get
            {
                return typeof(U);
            }
        }

        /// <summary>
        /// Dynamic data for Message
        /// </summary>
        public U Data { get; }

        /// <summary>
        /// Can contain the original sender of the message
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
