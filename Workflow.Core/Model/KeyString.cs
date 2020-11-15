using System;
using System.Collections.Generic;
using System.Text;
using DevelApp.Workflow.Core.Exceptions;

namespace DevelApp.Workflow.Core.Model
{
    public sealed class KeyString
    {
        private static int MAX_KEY_LENGTH = 100;
        private string _innerKeyString;

        public KeyString(string keyString)
        {
            if(keyString.Length > MAX_KEY_LENGTH)
            {
                throw new KeyStringException($"KeyString is above the allowed {MAX_KEY_LENGTH}");
            }
            _innerKeyString = keyString;
        }

        public override string ToString()
        {
            return _innerKeyString;
        }

        #region Implicit operators

        public static implicit operator KeyString(string rhs)
        {
            return new KeyString(rhs);
        }

        public static implicit operator string(KeyString rhs)
        {
            return rhs.ToString();
        }

        #endregion
    }
}
