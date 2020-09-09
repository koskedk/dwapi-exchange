namespace Dwapi.Exchange.SharedKernel
{
    public interface IExtractReader
    {
        PagedExtract Read(IExtractDefinition definition, int pageNumber,int pageSize);
    }
}
