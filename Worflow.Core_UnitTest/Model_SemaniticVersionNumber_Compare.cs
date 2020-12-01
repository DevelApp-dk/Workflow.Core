using System;
using Xunit;
using DevelApp.Workflow.Core.Model;
using Newtonsoft.Json;
using Shouldly;

namespace Worflow.Core_UnitTest
{
    public class Model_SemaniticVersionNumber_Compare
    {
        [Fact]
        public void SemanticVersionNumber_Compare_Major()
        {
            SemanticVersionNumber version = "2.3.4";
            SemanticVersionNumber lowerVersion = "1.3.4";
            SemanticVersionNumber higherVersion = "3.3.4";
            SemanticVersionNumber equalVersion = "2.3.4";

            version.CompareTo(lowerVersion).ShouldBe(1);
            version.CompareTo(higherVersion).ShouldBe(-1);
            version.CompareTo(equalVersion).ShouldBe(0);
        }
    }
}
