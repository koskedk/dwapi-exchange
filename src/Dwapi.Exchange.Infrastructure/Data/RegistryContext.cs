using System.Linq;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Exchange.Infrastructure.Data
{
    public class RegistryContext : BaseContext
    {
        public DbSet<Registry> Registries { get; set; }
        public DbSet<ExtractRequest> ExtractRequests { get; set; }
        public RegistryContext(DbContextOptions<RegistryContext> options) : base(options)
        {
        }
        public override void EnsureSeeded()
        {
            if (!Registries.Any())
            {
                var data = SeedDataReader.ReadCsv<Registry>(typeof(RegistryContext).Assembly);
                AddRange(data);
            }

            if (!ExtractRequests.Any())
            {
                var data = SeedDataReader.ReadCsv<ExtractRequest>(typeof(RegistryContext).Assembly);
                AddRange(data);
            }
            SaveChanges();
        }
    }
}
