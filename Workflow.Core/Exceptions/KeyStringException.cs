using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    internal class KeyStringException : Exception
    {
        public KeyStringException()
        {
        }

        public KeyStringException(string message) : base(message)
        {
        }

        public KeyStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KeyStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}