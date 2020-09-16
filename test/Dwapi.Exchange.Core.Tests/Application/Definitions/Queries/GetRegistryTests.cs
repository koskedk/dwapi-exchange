using System.Collections.Generic;
using System.Linq;
using Dwapi.Exchange.Core.Application.Definitions.Queries;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.Exchange.Core.Tests.Application.Definitions.Queries
{
    [TestFixture]
    public class GetRegistryTests
    {
        private List<Registry> _registries;
        private IMediator _mediator;

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
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_Get_Registry()
        {
            var result = _mediator.Send(new GetRegistry()).Result;
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Any());
        }
    }
}
