using System.Collections.Generic;
using System.Linq;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Infrastructure.Tests.TestArtifacts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.Exchange.Infrastructure.Tests.Data
{
    [TestFixture]
    public class RegistryRepositoryTests
    {
        private IRegistryRepository  _repository;
        private List<Registry> _registries;

        [OneTimeSetUp]
        public void Init()
        {
            _registries = TestData.GenerateRegistry();
            TestInitializer.ClearDb();
            TestInitializer.SeedData(_registries);
        }

        [SetUp]
        public void SetUp()
        {
            _repository = TestInitializer.ServiceProvider.GetService<IRegistryRepository>();
        }

        [Test]
        public void should_Get_By_Code()
        {
            var entities = _repository.GetByCode("dwh").Result;
            Assert.NotNull(entities);
            Assert.True(entities.ExtractRequests.Any());
        }
    }
}
