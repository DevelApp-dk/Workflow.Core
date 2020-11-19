using Akka.Actor;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Core.Messages
{
    public class DeadletterHandlingMessage
    {
        public DeadletterHandlingMessage(ReadOnlyCollection<(string Name, ActorPath ActorPath)> recipientList, object message)
        {
            RecipientList = recipientList;
            Message = message;
        }

        /// <summary>
        /// Returns the recipientlist with topmost owner at the top
        /// </summary>
        public ReadOnlyCollection<(string Name, ActorPath ActorPath)> RecipientList { get; }

        /// <summary>
        /// The message that was tried to be sent to recipient
        /// </summary>
        public object Message { get; }
    }
}
