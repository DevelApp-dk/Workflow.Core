using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DevelApp.Workflow.Core.Model
{
    /// <summary>
    /// Domain class representing version number
    /// </summary>
    public sealed class VersionNumber
    {
        private int _innerVersionNumber;
        public VersionNumber(int versionNumber)
        {
            _innerVersionNumber = versionNumber;
        }

        public int GetVersionNumber
        {
            get
            {
                return _innerVersionNumber;
            }
        }

        public override string ToString()
        {
            return _innerVersionNumber.ToString();
        }

        #region Implicit operators

        public static implicit operator VersionNumber(int rhs)
        {
            return new VersionNumber(rhs);

        }

        public static implicit operator int(VersionNumber rhs)
        {
            return rhs.GetVersionNumber;

        }

        #endregion
    }
}
