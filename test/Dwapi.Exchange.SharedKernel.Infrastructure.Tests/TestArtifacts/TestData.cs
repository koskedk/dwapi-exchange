using System;
using System.Collections.Generic;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public class TestData
    {
        public static List<TestCar> GenerateSample()
        {
            return new List<TestCar>
            {
                new TestCar("Subaru", "Legacy"),
                new TestCar("Toyoya", "Hilux")
            };
        }

        public static SampleDefinition GenerateSampleDefinition()
        {
            return new SampleDefinition
            {
                Name = @"Patient Extracts",
                Description = @"All Patients",
                SqlScript = @"select * from Patients",
                RecordCount = 10,
                Updated = DateTime.Now.AddHours(1),
                Refreshed = DateTime.Now
            };
        }
    }
}