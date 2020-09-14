using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.Core.Domain.Definitions
{
    public class Registry:Entity<Guid>
    {
        public string Name { get; set; }
        public string Purpose { get; set; }
        public ICollection<ExtractRequest> Requests { get; set; }=new List<ExtractRequest>();
    }
}