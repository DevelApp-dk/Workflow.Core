using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    internal class TransactionGroupIdException : Exception
    {
        public TransactionGroupIdException()
        {
        }

        public TransactionGroupIdException(string message) : base(message)
        {
        }

        public TransactionGroupIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TransactionGroupIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}