using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.Core.Domain.Definitions
{
    public class ExtractRequest : ExtractDefinition
    {
        public Guid RegistryId { get; set; }
    }
}
