using System.Threading.Tasks;

namespace Dwapi.Exchange.SharedKernel
{
    public interface IExtractReader
    {
        Task<PagedExtract> Read(ExtractDefinition definition, int pageNumber,int pageSize);
    }
}
