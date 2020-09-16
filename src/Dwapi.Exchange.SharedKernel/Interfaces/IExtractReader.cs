using System.Threading.Tasks;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.SharedKernel.Interfaces
{
    public interface IExtractReader
    {
        Task<PagedExtract> Read(ExtractDefinition definition, int pageNumber,int pageSize);
        Task<long> GetCount(ExtractDefinition definition);
        Task Initialize(ExtractDefinition definition);
    }
}
