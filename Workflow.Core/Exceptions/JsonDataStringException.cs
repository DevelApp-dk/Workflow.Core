using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    internal class JsonDataStringException : Exception
    {
        public JsonDataStringException()
        {
        }

        public JsonDataStringException(string message) : base(message)
        {
        }

        public JsonDataStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JsonDataStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}