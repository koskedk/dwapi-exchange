using System.Collections.Generic;
using System.Linq;
using Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests.Data
{
    [TestFixture]
    public class RepositoryTests
    {
        private ITestCarRepository _testEntityRepository;
        private List<TestCar> _testEntities;

        [OneTimeSetUp]
        public void Init()
        {
            _testEntities = TestData.GenerateSample();
            TestInitializer.ClearDb();
            TestInitializer.SeedData(_testEntities);
        }

        [SetUp]
        public void SetUp()
        {
            _testEntityRepository = TestInitializer.ServiceProvider.GetService<ITestCarRepository>();
        }

        [Test]
        public void should_Get()
        {
            var entity = _testEntityRepository.GetAsync(_testEntities.First().Id).Result;

            Assert.NotNull(entity);

            Log.Debug(entity.ToString());
        }

        [Test]
        public void should_Search()
        {
            var entities = _testEntityRepository
                .SearchAsync(x => x.Brand == _testEntities.First().Brand)
                .Result
                .ToList();

            Assert.True(entities.Any());

            foreach (var entity in entities)
                Log.Debug(entity.ToString());
        }

        [Test]
        public void should_Create()
        {
            var testEntity = new TestCar("Velar", "X");
            _testEntityRepository.CreateAsync(testEntity).Wait();

            var newTestEntity = _testEntityRepository.GetAsync(testEntity.Id).Result;
            Assert.NotNull(newTestEntity);
            Log.Debug(newTestEntity.ToString());
        }

        [Test]
        public void should_Update()
        {
            var testEntityForUpdate = _testEntities.First();
            testEntityForUpdate.Name = "GLE Benzx";


            _testEntityRepository.UpdateAsync(testEntityForUpdate).Wait();

            var updatedTestEntity = _testEntityRepository.GetAsync(testEntityForUpdate.Id).Result;
            Assert.AreEqual("GLE Benzx", updatedTestEntity.Name);
            Log.Debug(updatedTestEntity.ToString());
        }

        [Test]
        public void should_Delete_Existing_By_Id()
        {
            var testEntityForDelete = _testEntities.Last();

            _testEntityRepository.DeleteAsync(testEntityForDelete.Id);

            var deletedTestEntity = _testEntityRepository.GetAsync(testEntityForDelete.Id).Result;
            Assert.IsNull(deletedTestEntity);
        }
    }
}
