using System;
using System.Collections.Generic;
using System.Text;
using DevelApp.Workflow.Core.Exceptions;

namespace DevelApp.Workflow.Core.Model
{
    public sealed class VersionString
    {
        private static int MAX_VERSION_LENGTH = 100;
        private string _innerVersionString;

        public VersionString(string versionString)
        {
            if(versionString.Length > MAX_VERSION_LENGTH)
            {
                throw new KeyStringException($"KeyString is above the allowed {MAX_VERSION_LENGTH}");
            }
            _innerVersionString = versionString;
        }

        public override string ToString()
        {
            return _innerVersionString;
        }

        #region Implicit operators

        public static implicit operator VersionString(string rhs)
        {
            return new VersionString(rhs);
        }

        public static implicit operator string(VersionString rhs)
        {
            return rhs.ToString();
        }

        #endregion
    }
}
