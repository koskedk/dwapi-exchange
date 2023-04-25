using System;

namespace Dwapi.Exchange.Core.Domain.Definitions.Dtos
{
    public class EmrRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int[] SiteCode { get; set; }
        public DateTime? EvaluationDate { get; set; }
        public string CccNumber { get; set; }
        public int [] period { get; set;}
        public string indicatorName { get; set; }
        public string recencyId { get; set; }
    }
}