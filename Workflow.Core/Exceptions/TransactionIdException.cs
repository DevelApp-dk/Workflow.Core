using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    internal class TransactionIdException : Exception
    {
        public TransactionIdException()
        {
        }

        public TransactionIdException(string message) : base(message)
        {
        }

        public TransactionIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TransactionIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}