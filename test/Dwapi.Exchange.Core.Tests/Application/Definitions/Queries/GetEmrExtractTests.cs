using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using Dwapi.Exchange.Core.Application.Definitions.Queries;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Core.Domain.Definitions.Dtos;
using Dwapi.Exchange.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.Core.Tests.Application.Definitions.Queries
{
    [TestFixture]
    public class GetEmrExtractTests
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

        [TestCase("dwh","predictions",1,5,12602,5,"")]
        [TestCase("dwh","predictions",1,5,12602,0,"xx")]
        public void should_Get_Emr_Extract(string code,string name,int pageNumber,int pageSize,int siteCode,int total, string ccc)
        {
            var request = new EmrRequestDto
            {
                Code = code, Name = name, PageNumber = pageNumber, PageSize = pageSize,SiteCode =new []{siteCode} ,CccNumber = ccc
            };
            var getExtract = new GetEmrExtract(request);

            var result = _mediator.Send(getExtract).Result;
            Assert.True(result.IsSuccess);
            var top5 = result.Value;;
            Assert.AreEqual(total,top5.TotalItemCount);
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));
        }
    }
}
