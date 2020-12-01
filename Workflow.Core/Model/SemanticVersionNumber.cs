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
    public sealed class SemanticVersionNumber: IEquatable<SemanticVersionNumber>, IComparable<SemanticVersionNumber>
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        public SemanticVersionNumber(int major, int minor, int patch)
        {
            if(major < 0 || minor < 0 || patch < 0)
            {
                throw new SemanticVersionStringException($"A version number is below 0: {major}.{minor}.{patch}");
            }

            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }

        public SemanticVersionNumber Clone()
        {
            return new SemanticVersionNumber(Major, Minor, Patch);
        }

        public bool Equals(SemanticVersionNumber other)
        {
            if(Major == other.Major && Minor == other.Minor && Patch == other.Patch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CompareTo(SemanticVersionNumber other)
        {
            if (Major != other.Major)
            {
                return Math.Sign(Major - other.Major);
            }
            if (Minor != other.Minor)
            {
                return Math.Sign(Minor - other.Minor);
            }
            if (Patch != other.Patch)
            {
                return Math.Sign(Patch - other.Patch);
            }
            return 0;
        }

        #region Implicit operators

        public static implicit operator SemanticVersionNumber(string rhs)
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

            return new SemanticVersionNumber(major, minor, patch);
        }

        public static implicit operator string(SemanticVersionNumber rhs)
        {
            return rhs.ToString();
        }

        public static implicit operator Version(SemanticVersionNumber rhs)
        {
            return new Version(rhs.Major, rhs.Minor, rhs.Patch);
        }

        public static implicit operator SemanticVersionNumber(Version rhs)
        {
            return new SemanticVersionNumber(rhs.Major, rhs.Minor, rhs.Build);
        }

        #endregion
    }
}
