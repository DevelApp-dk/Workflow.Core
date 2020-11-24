using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    internal class SemanticVersionStringException : Exception
    {
        public SemanticVersionStringException()
        {
        }

        public SemanticVersionStringException(string message) : base(message)
        {
        }

        public SemanticVersionStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SemanticVersionStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}