using System;

namespace Dwapi.Exchange.Core.Domain.Definitions.Dtos
{
    public class ExtractRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long RecordCount { get; set; }
    }
}
