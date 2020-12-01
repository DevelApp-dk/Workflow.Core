using DevelApp.Workflow.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.Model
{
    public class TransactionId:IEquatable<TransactionId>
    {
        private Guid _guid;

        public TransactionId()
        {
            _guid = Guid.NewGuid();
        }

        public TransactionId(Guid transactionId)
        {
            _guid = transactionId;
        }

        public TransactionId(string transactionId)
        {
            try
            {
                _guid = Guid.Parse(transactionId);
            }
            catch(Exception ex)
            {
                throw new TransactionIdException($"The supplied transactionId string is not a valid format: |{transactionId}| should have been the format |{Guid.Empty.ToString().Replace('0','X')}|", ex);
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

        public TransactionId Clone()
        {
            return new TransactionId(this.ToString());
        }

        public bool Equals(TransactionId other)
        {
            return ToString().Equals(other.ToString());
        }

        #region Implicit operators

        public static implicit operator TransactionId(string rhs)
        {
            return new TransactionId(rhs);
        }

        public static implicit operator string(TransactionId rhs)
        {
            return rhs.ToString();
        }

        public static implicit operator TransactionId(Guid rhs)
        {
            return new TransactionId(rhs);
        }

        public static implicit operator Guid(TransactionId rhs)
        {
            return rhs._guid;
        }

        #endregion

    }
}
