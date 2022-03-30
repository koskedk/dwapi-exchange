using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.SharedKernel.Interfaces
{
    public interface IExtractReader
    {
        Task<PagedExtract> Read(ExtractDefinition definition, int pageNumber,int pageSize);
        Task<PagedExtract> Read(ExtractDefinition definition, int pageNumber,int pageSize,int[] siteCode = null);
        Task<PagedExtract> ReadProc(ExtractDefinition definition, int pageNumber,int pageSize);
        Task<PagedExtract> ReadProfile(ExtractDefinition definition, int pageNumber, int pageSize, int[] siteCode = null, string[] county = null);
        Task<PagedExtract> ReadProfileFilter(ExtractDefinition definition, int pageNumber, int pageSize, int[] siteCode = null, string[] county = null, string gender="", int age=-1);
        Task<PagedProfileExtract> ReadProfileFilterExpress(ExtractDefinition mainDefinition,ExtractDefinition definition, int pageNumber, int pageSize, int[] siteCode = null, string[] county = null, string gender="", int age=-1);
        Task<long> GetCount(ExtractDefinition definition);
        Task<long> GetCountFrom(ExtractDefinition definition,string fromSource,int[] siteCode);
        Task Initialize(ExtractDefinition definition);
    }
}
