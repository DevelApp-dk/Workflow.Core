using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using Akka.Persistence;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Core.AbstractActors
{
    /// <summary>
    /// Used as a base class for persisted actors with automatic restore of individual items typically messages 
    /// and a actor state (which can be a class or a collection based on the individual items)
    /// </summary>
    /// <typeparam name="IndividualItem"></typeparam>
    /// <typeparam name="ActorState"></typeparam>
    public abstract class AbstractPersistedWorkflowActor<IndividualItem, ActorState> : ReceivePersistentActor where ActorState : class, new()
    {
        protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);
        protected ActorState State;

        public AbstractPersistedWorkflowActor(int actorInstance = 1, int snapshotPerVersion = 100)
        {
            ActorInstance = actorInstance;

            _snapshotPerVersion = snapshotPerVersion;

            State = new ActorState();

            //Recover
            Recover<IndividualItem>(data => {
                Logger.Debug("{0} recovered data", ActorId);
                RecoverPersistedWorkflowDataHandler(data);
            });

            Recover<SnapshotOffer>(offer => {
                Logger.Debug("{0} offered snapshot", ActorId);
                ActorState data = offer.Snapshot as ActorState;
                RecoverPersistedSnapshotWorkflowDataHandler(data);
            });

            //Commands (like Receive)
            Command<IWorkflowMessage>(message => {
                Context.IncrementMessagesReceived();
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                bool notHandled = true;
                if(message.IsReply || message.IsTimeout)
                {
                    notHandled = HandleMessageReplies(message);
                }
                if (notHandled)
                {
                    WorkflowMessageHandler(message);
                }
            });

            Command<GroupFinishedMessage>(message => {
                GroupFinishedMessageHandler(message);
            });

            Command<SaveSnapshotSuccess>(success => {
                Logger.Debug("SaveSnapshot succeeded for {0} so deleting messages until this snapshot", PersistenceId);
                // soft-delete the journal up until the sequence # at
                // which the snapshot was taken
                DeleteMessages(success.Metadata.SequenceNr);
            });

            //Handle snapshot failue
            Command<SaveSnapshotFailure>(failure =>
            {
                Logger.Debug(failure.Cause, "SaveSnapshot Failed for {0}", PersistenceId);
            });

            //Handle snapshot failue
            Command<DeleteMessagesFailure>(failure =>
            {
                Logger.Debug(failure.Cause, "DeleteMessages Failed for {0}", PersistenceId);
            });

            //Handle deleted messages success
            Command<DeleteMessagesSuccess>(message =>
            {
                //Do nothing
            }
            );

            Command<DeadletterHandlingMessage>(message => {
                Context.IncrementCounter(nameof(DeadletterHandlingMessage));
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                DeadletterHandlingMessageHandler(message);
            });
        }

        /// <summary>
        /// When telling a group of messages this is where you handle replies whne more than one or messages has timed out
        /// </summary>
        /// <param name="message"></param>
        protected abstract void GroupFinishedMessageHandler(GroupFinishedMessage message);

        #region Actor Idenitifaction

        private string _actorId;

        /// <summary>
        /// Returns the unique actor id
        /// </summary>
        public virtual string ActorId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_actorId))
                {
                    _actorId = $"{ActorName}_{ActorVersion}_{ActorInstance}";
                }

                return _actorId;
            }
        }

        private string _actorName;

        /// <summary>
        /// ActorName is typically the Key for the actor. Override if not classname without Actor
        /// </summary>
        public virtual KeyString ActorName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_actorName))
                {
                    _actorName = GetType().Name.Replace("Actor", "");
                }
                return _actorName;
            }
        }

        private SemanticVersionNumber _semanticVersionNumber;

        /// <summary>
        /// Returns the actor version based on the assembly version
        /// </summary>
        public SemanticVersionNumber ActorVersion
        {
            get
            {
                if (_semanticVersionNumber == null)
                {
                    _semanticVersionNumber = GetType().Assembly.GetName().Version;
                }
                return _semanticVersionNumber;
            }
        }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        public int ActorInstance { get; }

        /// <summary>
        /// Returns the persistant name as default. Override on specific needs
        /// </summary>
        public override string PersistenceId
        {
            get
            {
                return ActorId;
            }
        }

        #endregion

        #region Message handling

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
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(IWorkflowMessage message);

        #endregion

        #region Lifetime handling

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

        #endregion

        #region Persitence Handling

        private int _snapshotPerVersion;
        private int _persistsSinceLastSnapshot;

        //TODO Handle persist errors. Perhaps degraded

        protected override void OnReplaySuccess()
        {
            DoLastActionsAfterRecover();
            base.OnReplaySuccess();
        }

        /// <summary>
        /// Called when a recover succeeded
        /// </summary>
        protected abstract void DoLastActionsAfterRecover();

        /// <summary>
        /// Persists data in versions until snapshotPerVersion
        /// </summary>
        /// <param name="data"></param>
        protected void PersistWorkflowData(IndividualItem data)
        {
            if (_snapshotPerVersion <= 1)
            {
                SaveSnapshot(State);
            }
            else
            {
                Persist(data, s =>
                {
                    if (++_persistsSinceLastSnapshot % _snapshotPerVersion == 0)
                    {
                        //time to save a snapshot
                        SaveSnapshot(State);
                    }
                });
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
            foreach((IActorRef recipient, IMessage message) tuple in recipientList)
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
        /// Used for recovering from crash
        /// </summary>
        /// <param name="data"></param>
        protected abstract void RecoverPersistedWorkflowDataHandler(IndividualItem dataItem);

        /// <summary>
        /// Snapshot is offered to start Recover process
        /// </summary>
        /// <param name="data"></param>
        private void RecoverPersistedSnapshotWorkflowDataHandler(ActorState persistedCollection)
        {
            State = persistedCollection;
        }

        #endregion

        #region Deadletter handling

        /// <summary>
        /// Handles DeadletterHandlingMessage. Default is to log and ignore
        /// </summary>
        /// <param name="message"></param>
        protected virtual void DeadletterHandlingMessageHandler(DeadletterHandlingMessage message)
        {
            Logger.Debug("{0} received message {1}", ActorId, message.ToString());
        }

        #endregion
    }
}
