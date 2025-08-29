using System.Linq;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.IntegrationTests.Data
{
    [TestFixture]
    public class ExtractReaderTests
    {
        private IExtractReader _reader;
        private IRegistryRepository _repository;
        [SetUp]
        public void SetUp()
        {
            _reader = TestInitializer.ServiceProvider.GetService<IExtractReader>();
            _repository = TestInitializer.ServiceProvider.GetService<IRegistryRepository>();
        }
        
        [TestCase("AYP","Indicators",new[]{"Initiated ART","Screened for TB"})]
        public void should_Read_By_Indicators(string code,string name,string[] indicators)
        {
            var filter = new DatasetFilter()
            {
                PageNumber = 1,PageSize = 5,
                Indicators = indicators
            };
            
            var registry =  _repository.GetByCode(code).Result;
            Assert.IsNotNull(registry);
            var request = registry.GetRequestByDef(name);
            Assert.IsNotNull(request);
            var top5 = _reader.Read(request,filter).Result;
            Assert.AreEqual(1,top5.PageNumber);
            Assert.AreEqual(5,top5.PageSize);
            Assert.That(top5.PageCount,Is.GreaterThan(0));
            Assert.That(top5.TotalItemCount,Is.GreaterThan(0));
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));
        }
        
        [TestCase("AYP","Indicators", new[]{15311,11614},new[]{"Initiated ART","Screened for TB"})]
        public void should_Read_By_Site_Indicators(string code,string name,int[] mflCodes,string[] indicators)
        {
            var filter = new DatasetFilter()
            { 
                PageNumber = 1,PageSize = 5,
                SiteCodes = mflCodes,
                Indicators = indicators
            };
            
            var registry =  _repository.GetByCode(code).Result;
            Assert.IsNotNull(registry);
            var request = registry.GetRequestByDef(name);
            Assert.IsNotNull(request);
            var top5 = _reader.Read(request,filter).Result;
            Assert.AreEqual(1,top5.PageNumber);
            Assert.AreEqual(5,top5.PageSize);
            Assert.That(top5.PageCount,Is.GreaterThan(0));
            Assert.That(top5.TotalItemCount,Is.GreaterThan(0));
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));
        }
    }
}
