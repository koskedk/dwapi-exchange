using System;
using System.Collections.Generic;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.SharedKernel.Custom;

namespace Dwapi.Exchange.Core.Tests.TestArtifacts
{
    public class TestData
    {
        public static List<Registry> GenerateRegistry()
        {
            var registry = new Registry
            {
                Id = LiveGuid.NewGuid(),
                Name = "DWH", Purpose = "DWH", Code = "DWH"
            };
            registry.AddRequest(GenerateExtractRequest());
            return new List<Registry>() {registry};
        }

        public static List<Registry> GenerateRawRegistry()
        {
            var registry = new Registry
            {
                Id = LiveGuid.NewGuid(),
                Name = "DWHX", Purpose = "DWHX", Code = "DWHX"
            };
            registry.AddRequest(GenerateExtractRequest(0));
            return new List<Registry>() {registry};
        }
        public static ExtractRequest GenerateExtractRequest(int count=10)
        {
            return new ExtractRequest
            {
                Id = LiveGuid.NewGuid(),
                Name = @"Patients",
                Description = @"All Patients",
                SqlScript = @"select * from Patients",
                RecordCount = count,
                Updated = DateTime.Now.AddHours(1)
            };
        }
    }
}
