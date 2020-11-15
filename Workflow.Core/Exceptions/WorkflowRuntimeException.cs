using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    public class WorkflowRuntimeException : Exception
    {
        public WorkflowRuntimeException()
        {
        }

        public WorkflowRuntimeException(string message) : base(message)
        {
        }

        public WorkflowRuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WorkflowRuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}