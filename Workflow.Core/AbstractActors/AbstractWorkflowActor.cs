﻿using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevelApp.Workflow.Core.AbstractActors
{
    public abstract class AbstractWorkflowActor : ReceiveActor
    {
        protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);

        public AbstractWorkflowActor(int actorInstance = 1)
        {
            ActorInstance = actorInstance;

            Receive<IWorkflowMessage>(message => {
                Context.IncrementMessagesReceived();
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                WorkflowMessageHandler(message);
            });

            Receive<DeadletterHandlingMessage>(message => {
                Context.IncrementCounter(nameof(DeadletterHandlingMessage));
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                DeadletterHandlingMessageHandler(message);
            });
        }

        /// <summary>
        /// Returns the unique actor id
        /// </summary>
        protected virtual string ActorId
        {
            get
            {
                return GetType().Name.Replace("Actor", "") + $"_{ActorVersion}_{ActorInstance}";
            }
        }

        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        protected abstract VersionNumber ActorVersion { get; }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        protected int ActorInstance { get; }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PreStart()
        {
            base.PreStart();
            Context.IncrementActorCreated();
        }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PostStop()
        {
            Context.IncrementActorStopped();
            base.PostStop();
        }

        Dictionary<Guid, Dictionary<Guid, IMessage>> expectedReplies = new Dictionary<Guid, Dictionary<Guid, IMessage>>();

        /// <summary>
        /// Handles reply messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleMessageReplies(IMessage message)
        {
            if (message.IsTimeout)
            {
                if (expectedReplies.TryGetValue(message.TransactionGroupId, out Dictionary<Guid, IMessage> transactionGroup))
                {
                    expectedReplies.Remove(message.TransactionGroupId);
                    Self.Tell(new GroupFinishedMessage(message.TransactionGroupId, transactionGroup), ActorRefs.NoSender);
                }
                return true;
            }
            else
            {
                if (expectedReplies.TryGetValue(message.TransactionGroupId, out Dictionary<Guid, IMessage> transactionGroup))
                {
                    if (transactionGroup.ContainsKey(message.TransactionId))
                    {
                        transactionGroup[message.TransactionId] = message;
                    }
                    if (transactionGroup.Values.All(m => m != null))
                    {
                        expectedReplies.Remove(message.TransactionGroupId);
                        if (transactionGroup.Count == 1)
                        {
                            //Why send another message ?
                            return false;
                        }
                        else
                        {
                            Self.Tell(new GroupFinishedMessage(message.TransactionGroupId, transactionGroup), ActorRefs.NoSender);
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Tell and expect a reply sometime in the future. A variation of the ask
        /// </summary>
        /// <param name="transactionGroupId"></param>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        protected void TellWithReply(Guid transactionGroupId, IActorRef recipient, IMessage message)
        {
            TellGroupWithReply(transactionGroupId, new List<(IActorRef recipient, IMessage message)>() { (recipient, message) });
        }

        /// <summary>
        /// Tell and expect a consolidated reply with all messages
        /// </summary>
        /// <param name="transactionGroupId"></param>
        /// <param name="recipientList"></param>
        protected void TellGroupWithReply(Guid transactionGroupId, List<(IActorRef recipient, IMessage message)> recipientList)
        {
            Dictionary<Guid, IMessage> group = new Dictionary<Guid, IMessage>();
            foreach ((IActorRef recipient, IMessage message) tuple in recipientList)
            {
                group.Add(tuple.message.TransactionId, null);
            }
            expectedReplies.Add(transactionGroupId, group);
            foreach ((IActorRef recipient, IMessage message) tuple in recipientList)
            {
                tuple.recipient.Tell(tuple.message);
            }
        }

        /// <summary>
        /// Tell and expect a reply sometime in the future. Will timeout and ignore messages received too late
        /// </summary>
        /// <param name="transactionGroupId"></param>
        /// <param name="timeout"></param>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        protected void TellWithReplyAndTimeout(Guid transactionGroupId, TimeSpan timeout, IActorRef recipient, IMessage message)
        {
            TellGroupWithReplyAndTimeout(transactionGroupId, timeout, new List<(IActorRef recipient, IMessage message)>() { (recipient, message) });
        }

        /// <summary>
        /// Tell and expect a consolidated reply with all messages. Will timeout and ignore messages received too late
        /// </summary>
        /// <param name="transactionGroupId"></param>
        /// <param name="timeout"></param>
        /// <param name="recipientList"></param>
        protected void TellGroupWithReplyAndTimeout(Guid transactionGroupId, TimeSpan timeout, List<(IActorRef recipient, IMessage message)> recipientList)
        {
            Dictionary<Guid, IMessage> group = new Dictionary<Guid, IMessage>();
            foreach ((IActorRef recipient, IMessage message) tuple in recipientList)
            {
                group.Add(tuple.message.TransactionId, null);
            }
            expectedReplies.Add(transactionGroupId, group);
            foreach ((IActorRef recipient, IMessage message) tuple in recipientList)
            {
                tuple.recipient.Tell(tuple.message);
            }
            Context.System.Scheduler.ScheduleTellOnce(timeout, Self, new GroupTimedOutMessage(transactionGroupId), Self);
        }

        /// <summary>
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(IWorkflowMessage message);

        /// <summary>
        /// Handles DeadletterHandlingMessage. Default is to log and ignore
        /// </summary>
        /// <param name="message"></param>
        protected virtual void DeadletterHandlingMessageHandler(DeadletterHandlingMessage message)
        {
            Logger.Debug("{0} received message {1}", ActorId, message.ToString());
        }
    }
}
