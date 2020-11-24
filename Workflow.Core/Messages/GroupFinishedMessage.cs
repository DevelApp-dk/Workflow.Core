using System;
using System.Collections.Generic;

namespace DevelApp.Workflow.Core.Messages
{
    public class GroupFinishedMessage
    {
        public Guid TransactionGroupId { get; }
        public Dictionary<Guid, IMessage> Messages { get; }

        public GroupFinishedMessage(Guid transactionGroupId, Dictionary<Guid, IMessage> messages)
        {
            TransactionGroupId = transactionGroupId;
            Messages = messages;
        }
    }
}