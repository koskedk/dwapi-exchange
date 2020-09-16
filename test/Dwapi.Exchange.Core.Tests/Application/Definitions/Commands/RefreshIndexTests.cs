using System.Collections.Generic;
using Dwapi.Exchange.Core.Application.Definitions.Commands;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.Exchange.Core.Tests.Application.Definitions.Commands
{
    [TestFixture]
    public class RefreshIndexTests
    {
        private List<Registry> _registries;
        private IMediator _mediator;
        private IRegistryRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            _registries = TestData.GenerateRawRegistry();
            TestInitializer.ClearDb();
            TestInitializer.SeedData(_registries);
        }

        [SetUp]
        public void SetUp()
        {
            _repository = TestInitializer.ServiceProvider.GetService<IRegistryRepository>();
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_Refresh_Index()
        {
            var result = _mediator.Send(new RefreshIndex("DWHX")).Result;

            Assert.True(result.IsSuccess);
            var registry = _repository.GetByCode("DWHX").Result;
            var ex = registry.GetRequestByDef("Patients");
            Assert.AreEqual(10,ex.RecordCount);
        }
    }
}
