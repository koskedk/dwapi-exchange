using System.Linq;
using Dwapi.Exchange.Core.Application.Definitions.Queries;
using Dwapi.Exchange.SharedKernel.Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.Core.IntegrationTests.Application.Definitions.Queries
{
    [TestFixture]
    public class GetIndicatorExtractTests
    {
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [TestCase("AYP", "IndicatorList", 1, 15, null, null)]
        [TestCase("AYP", "Indicators", 1, 5, null, null)]
        [TestCase("AYP", "Indicators", 1, 5, new[] { 15311, 11614 }, null)]
        [TestCase("AYP", "Indicators", 1, 5, null, new[] { "Initiated ART", "Screened for TB" })]
        [TestCase("AYP", "Indicators", 1, 5, new[] { 15311, 11614 }, new[] { "Initiated ART", "Screened for TB" })]
        public void should_Get_Extracts(string code, string name, int pageNumber, int pageSize, int[] siteCodes,
            string[] indicators)
        {
            var filter = new DatasetFilter()
            {
                Code = code, 
                Name = name, 
                PageNumber = pageNumber, 
                PageSize = pageSize, 
                SiteCodes =siteCodes,
                Indicators = indicators
            };
            var getExtract = new GetIndicatorExtract(filter);

            var result = _mediator.Send(getExtract).Result;
            Assert.True(result.IsSuccess);
            var top5 = result.Value; 
            Log.Debug(top5.ToString());
            if (top5.Extract.Any())
                Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));
        }
    }
}
