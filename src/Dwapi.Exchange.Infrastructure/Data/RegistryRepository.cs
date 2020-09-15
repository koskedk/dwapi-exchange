using System;
using System.Linq;
using System.Threading.Tasks;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.SharedKernel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Exchange.Infrastructure.Data
{
    public class RegistryRepository : Repository<Registry, Guid>, IRegistryRepository
    {
        public RegistryRepository(RegistryContext context) : base(context)
        {
        }

        public IQueryable<Registry> GetAll()
        {
            var ctx = Context as RegistryContext;

            return ctx
                .Registries
                .Include(x => x.ExtractRequests);
        }

        public async Task<Registry> GetByCode(string code)
        {
            var registry = await GetAll()
                .Where(x => x.Name.ToLower() == code)
                .FirstOrDefaultAsync();

            return registry;
        }
    }
}
