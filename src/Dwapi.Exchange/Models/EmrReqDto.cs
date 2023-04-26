using System;
using Microsoft.AspNetCore.Mvc;

namespace Dwapi.Exchange.Models
{
    public class EmrReqDto
    {
        [FromQuery(Name = "code")] public string code { get; set; }
        [FromQuery(Name = "name")] public string name { get; set; }
        [FromQuery(Name = "pageNumber")] public int pageNumber { get; set; }
        [FromQuery(Name = "pageSize")] public int pageSize { get; set; }
        [FromQuery(Name = "siteCode")] public int[] siteCode { get; set; }
        [FromQuery(Name = "evaluationDate")] public DateTime? evaluationDate { get; set; }
        [FromQuery(Name = "cccNumber")] public string cccNumber { get; set; }
        [FromQuery(Name = "recencyId")] public string recencyId { get; set; }
        [FromQuery(Name = "period")] public string[] period { get; set; }
        [FromQuery(Name = "indicatorName")] public string indicatorName { get; set; }
    }
}
