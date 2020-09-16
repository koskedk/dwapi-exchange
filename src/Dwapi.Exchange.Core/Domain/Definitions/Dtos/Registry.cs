using System;
using System.Collections.Generic;

namespace Dwapi.Exchange.Core.Domain.Definitions.Dtos
{
    public class RegistryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Code { get; set; }
        public List<ExtractRequestDto> ExtractRequests { get; set; } = new List<ExtractRequestDto>();
    }
}
