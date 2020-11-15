using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    internal class VersionStringException : Exception
    {
        public VersionStringException()
        {
        }

        public VersionStringException(string message) : base(message)
        {
        }

        public VersionStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VersionStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}