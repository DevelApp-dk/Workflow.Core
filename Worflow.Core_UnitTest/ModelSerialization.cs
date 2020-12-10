using System;
using Xunit;
using DevelApp.Workflow.Core.Model;
using Newtonsoft.Json;
using Shouldly;

namespace Worflow.Core_UnitTest
{
    public class ModelSerialization
    {
        [Fact]
        public void JsonDataString_Serialization()
        {
            JsonDataString jsonDataString = new JsonDataString("{ \"monster\": 20}");

            string output = JsonConvert.SerializeObject(jsonDataString);

            JsonDataString deserializedJsonDataString = JsonConvert.DeserializeObject<JsonDataString>(output);

            deserializedJsonDataString.ShouldBe(jsonDataString);
        }

        [Fact]
        public void TransactionGroupId_Serialization()
        {
            TransactionGroupId transactionGroupId = new TransactionGroupId(Guid.NewGuid().ToString());

            string output = JsonConvert.SerializeObject(transactionGroupId);

            TransactionGroupId deserializedTransactionGroupId = JsonConvert.DeserializeObject<TransactionGroupId>(output);

            deserializedTransactionGroupId.ShouldBe(transactionGroupId);
        }

        [Fact]
        public void TransactionId_Serialization()
        {
            TransactionId transactionId = new TransactionId(Guid.NewGuid().ToString());

            string output = JsonConvert.SerializeObject(transactionId);

            TransactionId deserializedTransactionId = JsonConvert.DeserializeObject<TransactionId>(output);

            deserializedTransactionId.ShouldBe(transactionId);
        }
    }
}
