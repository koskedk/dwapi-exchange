using System;
using Dwapi.Exchange.SharedKernel.Interfaces;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public interface ITestCarRepository : IRepository<TestCar, Guid>
    {

    }
}