using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts;
using Dwapi.Exchange.SharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests.Data
{
    [TestFixture]
    public class ExtractReaderTests
    {
        private IExtractReader _reader;
        private SampleDefinition _definition,_mainDefinition, _profileDefinition,_adhocDefination;


        [SetUp]
        public void SetUp()
        {
            _definition = TestData.GenerateSampleDefinition();
            _mainDefinition = TestData.GenerateMainSampleDefinition();
            _adhocDefination = TestData.GenerateAdhocSampleDefinition();
            _profileDefinition = TestData.GenerateSampleProfileDefinition();
            _reader = TestInitializer.ServiceProvider.GetService<IExtractReader>();
        }

        [Test]
        public void should_Read()
        {
            var top5 = _reader.Read(_definition,1, 5).Result;
            Assert.AreEqual(1,top5.PageNumber);
            Assert.AreEqual(5,top5.PageSize);
            Assert.AreEqual(2,top5.PageCount);
            Assert.AreEqual(5,top5.TotalItemCount);
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));


            var bottom5 = _reader.Read(_definition, 2, 5).Result;
            Assert.AreEqual(2,bottom5.PageNumber);
            Assert.AreEqual(5,bottom5.PageSize);
            Assert.AreEqual(2,bottom5.PageCount);
            Assert.AreEqual(5,bottom5.TotalItemCount);
            Log.Debug(bottom5.ToString());
            Log.Debug(JsonConvert.SerializeObject(bottom5.Extract.First()));
        }
        [Test]
        public void should_Read_Filters()
        {
            var siteCodes = new List<int>()
            {
                101,13634,15788
            };
            var counties = new List<string>();
            var top5 = _reader.Read(_adhocDefination,1, 5,null).Result;
            Assert.True(top5.TotalItemCount>0);
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));

            ///DataSource=/Users/koskedk/Projects/hmis/dwh/dwapi/exchange/test/Dwapi.Exchange.SharedKernel.Infrastructure.Tests/bin/Debug/netcoreapp3.1/TestArtifacts/Database/source.db

        }
        // [Test]
        public void should_Read_Profile()
        {
            var siteCodes = new List<int>()
            {
                101,13634
            };
            var counties = new List<string>();
            var top5 = _reader.ReadProfile(_definition,1, 5,siteCodes.ToArray(),counties.ToArray()).Result;
            Assert.AreEqual(1,top5.PageNumber);
            Assert.AreEqual(5,top5.PageSize);
            Assert.AreEqual(2,top5.PageCount);
            Assert.AreEqual(5,top5.TotalItemCount);
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));



        }


        // [Test]
        public void should_Read_Profile_Filters()
        {
            var siteCodes = new List<int>()
            {
                101
            };
            var counties = new List<string>();
            var top5 = _reader.ReadProfileFilter(_definition,1, 5,siteCodes.ToArray(),counties.ToArray(),"Male",10).Result;
            Assert.AreEqual(1,top5.PageNumber);
            Assert.AreEqual(5,top5.PageSize);
            Assert.AreEqual(2,top5.PageCount);
            Assert.AreEqual(5,top5.TotalItemCount);
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));

///DataSource=/Users/koskedk/Projects/hmis/dwh/dwapi/exchange/test/Dwapi.Exchange.SharedKernel.Infrastructure.Tests/bin/Debug/netcoreapp3.1/TestArtifacts/Database/source.db

        }

        [Test]
        public void should_Read_Profile_Filters_Slapper()
        {
            var top5 = _reader.ReadProfileFilterExpress(_mainDefinition, _profileDefinition,1, 4).Result;
            Assert.AreEqual(1,top5.PageNumber);
            Assert.AreEqual(4,top5.PageSize);
            Assert.AreEqual(1,top5.PageCount);
            Assert.AreEqual(4,top5.TotalItemCount);
            Log.Debug(top5.ToString());
            Log.Debug(JsonConvert.SerializeObject(top5.Extract.First()));

            ///DataSource=/Users/koskedk/Projects/hmis/dwh/dwapi/exchange/test/Dwapi.Exchange.SharedKernel.Infrastructure.Tests/bin/Debug/netcoreapp3.1/TestArtifacts/Database/source.db

        }

        [Test]
        public void should_Get_Count()
        {
            var top5 = _reader.GetCount(_definition).Result;
            Assert.AreEqual(10,top5);
        }

        [Test]
        public void should_Get_Count_Frm()
        {
            var sql = $"{_definition.SqlScript} limit 2";
            var top5 = _reader.GetCountFrom(_definition, sql,new []{1}).Result;
            Assert.AreEqual(2, top5);
        }
    }
}
