using DevelApp.Workflow.Core.Exceptions;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.Model
{
    public class JsonDataString
    {
        private JsonValue _innerJsonValue;

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
        public JsonValue JsonValue
        {
            get
            {
                return _innerJsonValue;
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
