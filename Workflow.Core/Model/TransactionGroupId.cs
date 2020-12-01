using DevelApp.Workflow.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.Model
{
    public class TransactionGroupId: IEquatable<TransactionGroupId>
    {
        private Guid _guid;

        public TransactionGroupId()
        {
            _guid = Guid.NewGuid();
        }

        public TransactionGroupId(Guid transactionGroupId)
        {
            _guid = transactionGroupId;
        }

        public TransactionGroupId(string transactionGroupId)
        {
            try
            {
                _guid = Guid.Parse(transactionGroupId);
            }
            catch(Exception ex)
            {
                throw new TransactionGroupIdException($"The supplied transactionGroupId string is not a valid format: |{transactionGroupId}| should have been the format |{Guid.Empty.ToString().Replace('0','X')}|", ex);
            }
        }

        /// <summary>
        /// Serialization property that might change as interface so please do not use
        /// </summary>
        public Guid InnerGuid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        public override string ToString()
        {
            return _guid.ToString();
        }

        public TransactionGroupId Clone()
        {
            return new TransactionGroupId(this.ToString());
        }

        public bool Equals(TransactionGroupId other)
        {
            return ToString().Equals(other.ToString());
        }

        #region Implicit operators

        public static implicit operator TransactionGroupId(string rhs)
        {
            return new TransactionGroupId(rhs);
        }

        public static implicit operator string(TransactionGroupId rhs)
        {
            return rhs.ToString();
        }

        public static implicit operator TransactionGroupId(Guid rhs)
        {
            return new TransactionGroupId(rhs);
        }

        public static implicit operator Guid(TransactionGroupId rhs)
        {
            return rhs._guid;
        }

        #endregion

    }
}
