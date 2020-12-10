using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Core.Actors
{
    public class DeadletterActor : ReceiveActor
    {
        private readonly ILoggingAdapter Logger = Logging.GetLogger(Context);

        public DeadletterActor(int actorInstance = 1)
        {
            ActorInstance = actorInstance;

            //Commands (like Receive)
            Receive<DeadLetter>(dl =>
            {
                Context.IncrementMessagesReceived();
                Logger.Debug("{0} received deadletter {1}", ActorId, dl.Message.ToString());
                DeadLetterMessageHandler(dl);
            });
        }

        #region Technical

        private string _actorId;

        /// <summary>
        /// Returns the unique actor id
        /// </summary>
        private string ActorId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_actorId))
                {
                    _actorId = GetType().Name.Replace("Actor", "") + $"_{ActorVersion}_{ActorInstance}";
                }
                return _actorId;
            }
        }

        private SemanticVersionNumber _actorVersion;
        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        private SemanticVersionNumber ActorVersion
        {
            get
            {
                if (_actorVersion == null)
                {
                    _actorVersion = GetType().Assembly.GetName().Version;
                }
                return _actorVersion;
            }
        }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        private int ActorInstance { get; }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PreStart()
        {
            base.PreStart();
            // subscribe to the event stream for messages of type "DeadLetter"
            Context.System.EventStream.Subscribe(Self, typeof(DeadLetter));
            Context.IncrementActorCreated();
        }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PostStop()
        {
            Context.IncrementActorStopped();
            Context.System.EventStream.Unsubscribe(Self, typeof(DeadLetter));
            base.PostStop();

        }
        #endregion

        /// <summary>
        /// Handles DeadLetters for workflow
        /// </summary>
        /// <param name="dl"></param>
        private void DeadLetterMessageHandler(DeadLetter dl)
        {
            Console.WriteLine($"DeadLetter captured: {dl.Message}, sender: {dl.Sender}, recipient: {dl.Recipient}");
            ReadOnlyCollection<(string Name, ActorPath ActorPath)> recipientList = WorkflowUtilities.DetermineRecipient(dl.Recipient);
            if (recipientList.Count == 0)
            {
                Console.WriteLine($"{GetType().Name} deadletter has odd empty actorPath so ignoring it");
            }
            else if (recipientList.Count > 0 && recipientList[0].ActorPath.Parent.Equals("system"))
            {
                Console.WriteLine($"{GetType().Name} deadletter is a system deadletter so ignoring it");
            }
            else
            {
                Context.ActorSelection(recipientList[0].ActorPath).Tell(new DeadletterHandlingMessage(recipientList, dl.Message), ActorRefs.NoSender);
            }
        }
    }
}
