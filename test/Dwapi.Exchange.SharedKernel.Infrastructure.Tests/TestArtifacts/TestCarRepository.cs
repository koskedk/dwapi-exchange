using System;
using Dwapi.Exchange.SharedKernel.Infrastructure.Data;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public class TestCarRepository : Repository<TestCar, Guid>, ITestCarRepository
    {
        public TestCarRepository(TestDbContext context) : base(context)
        {
        }
    }
}