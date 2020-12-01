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
        public void KeyString_Serialization()
        {
            KeyString keyString = new KeyString(Guid.NewGuid().ToString());

            string output = JsonConvert.SerializeObject(keyString);

            KeyString deserializedKeyString = JsonConvert.DeserializeObject<KeyString>(output);

            deserializedKeyString.ShouldBe(keyString);
        }

        [Fact]
        public void SemanticVersionNumber_Serialization()
        {
            SemanticVersionNumber semanticVersionNumber = new SemanticVersionNumber(2,3,4);

            string output = JsonConvert.SerializeObject(semanticVersionNumber);

            SemanticVersionNumber deserializedSemanticVersionNumber = JsonConvert.DeserializeObject<SemanticVersionNumber>(output);

            deserializedSemanticVersionNumber.ShouldBe(semanticVersionNumber);
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
