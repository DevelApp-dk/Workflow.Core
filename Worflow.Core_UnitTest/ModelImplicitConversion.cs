using System;
using Xunit;
using DevelApp.Workflow.Core.Model;
using Newtonsoft.Json;
using Manatee.Json;

namespace Worflow.Core_UnitTest
{
    public class ModelImplicitConversion
    {
        [Fact]
        public void JsonDataString_ImplicitConversion()
        {
            JsonDataString jsonDataString = "{ \"monster\": 20}";
            JsonDataString jsonDataStringJsonValue = new JsonValue("{ \"monster\": 20}");

            string outputString = jsonDataString;
            JsonValue jsonValue = jsonDataStringJsonValue;
        }

        [Fact]
        public void TransactionGroupId_ImplicitConversion()
        {
            TransactionGroupId transactionGroupId = Guid.NewGuid();
            TransactionGroupId transactionGroupId2 = Guid.NewGuid().ToString();

            string output = transactionGroupId;
            Guid outputGuid = transactionGroupId2;
        }

        [Fact]
        public void TransactionId_ImplicitConversion()
        {
            TransactionId transactionGroupId = Guid.NewGuid();
            TransactionId transactionGroupId2 = Guid.NewGuid().ToString();

            string output = transactionGroupId;
            Guid outputGuid = transactionGroupId2;
        }
    }
}
