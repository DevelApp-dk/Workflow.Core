using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DevelApp.Workflow.Core
{
    public static class WorkflowUtilities
    {
        /// <summary>
        /// Returns an ordered collection of actor hierarchy with top actor below user or system actors first
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<(string Name, ActorPath ActorPath)> DetermineRecipient(IActorRef recipient)
        {
            return DetermineRecipient(recipient.Path);
        }

        /// <summary>
        /// Returns an ordered collection of actor hierarchy with top actor below user or system actors first
        /// </summary>
        public static ReadOnlyCollection<(string Name, ActorPath ActorPath)> DetermineRecipient(ActorPath actorPath)
        {
            List<(string Name, ActorPath ActorPath)> recipientList = new List<(string Name, ActorPath ActorPath)>();
            // Strip all until user or system
            while (actorPath.Parent != null && !actorPath.Parent.Name.Equals("user") && !actorPath.Parent.Name.Equals("system"))
            {
                //Set actorPath to parent if not like root
                actorPath = actorPath.Parent;
                recipientList.Add((actorPath.Name, actorPath));
            }
            recipientList.Reverse();
            return recipientList.AsReadOnly();
        }
    }
}
