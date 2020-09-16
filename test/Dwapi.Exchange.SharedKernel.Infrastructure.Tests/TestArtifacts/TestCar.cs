using System;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public class TestCar : Entity<Guid>
    {
        public string Name { get; set; }
        public string Brand { get; set; }

        public TestCar()
        {
        }

        public TestCar(string name, string brand)
        {
            Name = name;
            Brand = brand;
        }

        public override string ToString()
        {
            return $"{Name} |{Id}";
        }
    }

    public class SampleDefinition : ExtractDefinition
    {
    }
}