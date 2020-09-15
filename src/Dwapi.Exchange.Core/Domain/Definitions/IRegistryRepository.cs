using System;
using System.Linq;
using System.Threading.Tasks;
using Dwapi.Exchange.SharedKernel.Interfaces;

namespace Dwapi.Exchange.Core.Domain.Definitions
{
    public interface IRegistryRepository : IRepository<Registry,Guid>
    {
        IQueryable<Registry> GetAll();
        Task<Registry> GetByCode(string code);
    }
}
