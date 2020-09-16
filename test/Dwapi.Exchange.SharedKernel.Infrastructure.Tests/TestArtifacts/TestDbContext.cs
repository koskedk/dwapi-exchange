using Microsoft.EntityFrameworkCore;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public class TestDbContext : DbContext
    {
        public DbSet<TestCar> TestCars { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
    }
}