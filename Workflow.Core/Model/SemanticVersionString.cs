using System;
using System.Collections.Generic;
using System.Text;
using DevelApp.Workflow.Core.Exceptions;

namespace DevelApp.Workflow.Core.Model
{
    /// <summary>
    /// Represent semantic versioning string consisting of
    /// Major: when you make incompatible API changes
    /// Minor: when you add functionality in a backwards compatible manner
    /// Patch: when you make backwards compatible bug fixes
    /// </summary>
    public sealed class SemanticVersionString
    {
        private int _major;
        private int _minor;
        private int _patch;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        public SemanticVersionString(int major, int minor, int patch)
        {
            if(major < 0 || minor < 0 || patch < 0)
            {
                throw new SemanticVersionStringException($"A version number is below 0: {major}.{minor}.{patch}");
            }

            _major = major;
            _minor = minor;
            _patch = patch;
        }

        public override string ToString()
        {
            return $"{_major}.{_minor}.{_patch}";
        }

        #region Implicit operators

        public static implicit operator SemanticVersionString(string rhs)
        {
            string[] parts = rhs.Split('.');
            if(parts.Length != 3)
            {
                throw new SemanticVersionStringException($"Versioning is not valid format major.minor.patch :{rhs}");
            }

            int major= -1;
            if(!int.TryParse(parts[0], out major))
            {
                throw new SemanticVersionStringException($"Versioning is not valid format for major :{parts[0]}");
            }
            int minor = -1;
            if (!int.TryParse(parts[1], out minor))
            {
                throw new SemanticVersionStringException($"Versioning is not valid format for minor :{parts[1]}");
            }
            int patch = -1;
            if (!int.TryParse(parts[2], out patch))
            {
                throw new SemanticVersionStringException($"Versioning is not valid format for patch :{parts[2]}");
            }

            return new SemanticVersionString(major, minor, patch);
        }

        public static implicit operator string(SemanticVersionString rhs)
        {
            return rhs.ToString();
        }

        #endregion
    }
}
