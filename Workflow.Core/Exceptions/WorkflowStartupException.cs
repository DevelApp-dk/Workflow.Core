using System;
using System.Runtime.Serialization;

namespace DevelApp.Workflow.Core.Exceptions
{
    [Serializable]
    public class WorkflowStartupException : Exception
    {
        public WorkflowStartupException()
        {
        }

        public WorkflowStartupException(string message) : base(message)
        {
        }

        public WorkflowStartupException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WorkflowStartupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}