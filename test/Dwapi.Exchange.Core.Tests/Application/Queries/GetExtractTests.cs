using System.Collections.Generic;
using System.Linq;
using Dwapi.Exchange.Core.Application.Queries;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.Core.Tests.Application.Queries
{
    [TestFixture]
    public class GetExtractTests
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
        public void should_Get_Extract()
        {
            var getExtract = new GetExtract("dwh","Patients",1,5);

            var result = _mediator.Send(getExtract).Result;
            Assert.True(result.IsSuccess);
            var top5 = result.Value;
            Assert.AreEqual(1,top5.PageNumber);
            Assert.AreEqual(5,top5.PageSize);
            Assert.AreEqual(2,top5.PageCount);
            Assert.AreEqual(5,top5.TotalItemCount);
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));
        }

    }
}
