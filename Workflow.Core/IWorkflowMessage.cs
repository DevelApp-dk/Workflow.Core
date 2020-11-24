using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    /// <summary>
    /// Workflow messages
    /// </summary>
    public interface IWorkflowMessage: IMessage
    {
        /// <summary>
        /// MessageTypeName is used to distinguish what the message is about
        /// </summary>
        string MessageTypeName { get; }
    }
}
