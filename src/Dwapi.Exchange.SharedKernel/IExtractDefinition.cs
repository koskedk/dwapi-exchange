using System.ComponentModel.DataAnnotations;

namespace Dwapi.Exchange.SharedKernel
{
    public interface IExtractDefinition
    {
        string Id { get; set; }
        string Description { get; set; }
        string Sql { get; set; }
        long RecordCount { get; set; }
    }
}
