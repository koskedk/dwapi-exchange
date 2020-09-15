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

        public static ExtractRequest GenerateExtractRequest()
        {
            return new ExtractRequest
            {
                Id = LiveGuid.NewGuid(),
                Name = @"Patients",
                Description = @"All Patients",
                SqlScript = @"select * from Patients",
                RecordCount = 10,
                Updated = DateTime.Now.AddHours(1),
                Refreshed = DateTime.Now
            };
        }
    }
}
