using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;

namespace DevelApp.Workflow.Core.Messages
{
    public class GroupFinishedMessage
    {
        public TransactionGroupId TransactionGroupId { get; }
        public Dictionary<TransactionId, IMessage> Messages { get; }

        public GroupFinishedMessage(Guid transactionGroupId, Dictionary<TransactionId, IMessage> messages)
        {
            TransactionGroupId = transactionGroupId;
            Messages = messages;
        }
    }
}