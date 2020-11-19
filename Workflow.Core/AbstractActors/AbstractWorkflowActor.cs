using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Core.AbstractActors
{
    public abstract class AbstractWorkflowActor : ReceiveActor
    {
        protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);

        public AbstractWorkflowActor(int actorInstance = 1)
        {
            ActorInstance = actorInstance;

            Receive<WorkflowMessage>(message => {
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

        /// <summary>
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(WorkflowMessage message);

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
