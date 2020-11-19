using System;
using System.Collections.Generic;
using System.Text;
using Akka.Event;
using Akka.Monitoring;
using Akka.Persistence;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Core.AbstractActors
{
    public abstract class AbstractPersistedWorkflowActor<IndividualItem, Collection> : ReceivePersistentActor where Collection : class, new()
    {
        protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);
        protected Collection PersistedCollection;

        public AbstractPersistedWorkflowActor(int actorInstance = 1, int snapshotPerVersion = 100)
        {
            ActorInstance = actorInstance;

            _snapshotPerVersion = snapshotPerVersion;

            PersistedCollection = new Collection();

            //Recover
            Recover<IndividualItem>(data => {
                Logger.Debug("{0} recovered data", ActorId);
                RecoverPersistedWorkflowDataHandler(data);
            });

            Recover<SnapshotOffer>(offer => {
                Logger.Debug("{0} offered snapshot", ActorId);
                Collection data = offer.Snapshot as Collection;
                RecoverPersistedSnapshotWorkflowDataHandler(data);
            });

            //Commands (like Receive)
            Command<IWorkflowMessage>(message => {
                Context.IncrementMessagesReceived();
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                WorkflowMessageHandler(message);
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
        /// Returns the unique actor id
        /// </summary>
        protected virtual string ActorId
        {
            get
            {
                return BuildInstanceName(ActorName, ActorVersion, ActorInstance);
            }
        }

        /// <summary>
        /// Returns Unique actor instance name
        /// </summary>
        /// <param name="actorName"></param>
        /// <param name="actorVersion"></param>
        /// <param name="actorInstance"></param>
        /// <returns></returns>
        protected string BuildInstanceName(KeyString actorName, VersionNumber actorVersion, int actorInstance = 1)
        {
            return $"{actorName}_{actorVersion}_{actorInstance}";
        }


        /// <summary>
        /// ActorName is typically the Key for the actor. Override if not classname without Actor
        /// </summary>
        public virtual KeyString ActorName
        {
            get
            {
                return GetType().Name.Replace("Actor", "");
            }
        }


        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        protected abstract VersionNumber ActorVersion { get; }

        /// <summary>
        /// Returns the persistant name as default. Override on 
        /// </summary>
        public override string PersistenceId
        {
            get
            {
                return ActorId;
            }
        }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        protected int ActorInstance { get; }

        #region Message handling

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
                SaveSnapshot(PersistedCollection);
            }
            else
            {
                Persist(data, s =>
                {
                    if (++_persistsSinceLastSnapshot % _snapshotPerVersion == 0)
                    {
                        //time to save a snapshot
                        SaveSnapshot(PersistedCollection);
                    }
                });
            }
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
        private void RecoverPersistedSnapshotWorkflowDataHandler(Collection persistedCollection)
        {
            PersistedCollection = persistedCollection;
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
