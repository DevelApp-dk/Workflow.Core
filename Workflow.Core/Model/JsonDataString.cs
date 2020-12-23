using DevelApp.Workflow.Core.Exceptions;
using Manatee.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.Model
{
    public sealed class JsonDataString: IEquatable<JsonDataString>
    {
        private JsonValue _innerJsonValue;

        public JsonDataString()
        {
            _innerJsonValue = JsonValue.Null;
        }

        public JsonDataString(string jsonString)
        {
            try
            {
                _innerJsonValue = JsonValue.Parse(jsonString);
            }
            catch (Exception ex)
            {
                throw new JsonDataStringException("The supplied jsonString is not valid",ex);
            }
        }

        public JsonDataString(JsonValue jsonValue)
        {
            _innerJsonValue = jsonValue;
        }

        /// <summary>
        /// Returns the inner 
        /// </summary>
        [JsonIgnore]
        public JsonValue JsonValue
        {
            get
            {
                return _innerJsonValue;
            }
        }

        /// <summary>
        /// Property for string representation of JsonValue
        /// </summary>
        public string Json
        {
            get
            {
                return ToString();
            }
            set
            {
                try
                {
                    _innerJsonValue = JsonValue.Parse(value);
                }
                catch (Exception ex)
                {
                    throw new JsonDataStringException("The supplied json is not valid", ex);
                }
            }
        }

        public override string ToString()
        {
            return _innerJsonValue.ToString();
        }

        /// <summary>
        /// Returns a nicely indented string
        /// </summary>
        /// <returns></returns>
        public string ToIndentedString()
        {
            return _innerJsonValue.GetIndentedString();
        }

        public JsonDataString Clone()
        {
            return new JsonDataString(this.ToString());
        }

        public bool Equals(JsonDataString other)
        {
            return _innerJsonValue.Equals(other.JsonValue);
        }

        public override int GetHashCode()
        {
            return -1468169951 + EqualityComparer<JsonValue>.Default.GetHashCode(_innerJsonValue);
        }

        #region Implicit operators

        public static implicit operator JsonDataString(string rhs)
        {
            return new JsonDataString(rhs);
        }

        public static implicit operator string(JsonDataString rhs)
        {
            return rhs.ToString();
        }

        public static implicit operator JsonDataString(JsonValue rhs)
        {
            return new JsonDataString(rhs);
        }

        public static implicit operator JsonValue(JsonDataString rhs)
        {
            return rhs.JsonValue;
        }

        #endregion
    }
}
