using System;
using System.Collections.Generic;
using System.Linq;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.Core.Domain.Definitions
{
    public class Registry : Entity<Guid>
    {
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Code { get; set; }
        public ICollection<ExtractRequest> ExtractRequests { get; set; } = new List<ExtractRequest>();

        public ExtractRequest GetRequestByDef(string defName)
        {
            return ExtractRequests.FirstOrDefault(x => x.Name.ToLower() == defName.ToLower());
        }

        public void AddRequest(ExtractRequest request)
        {
            request.RegistryId = Id;
            ExtractRequests.Add(request);
        }
    }
}
