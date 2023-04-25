using System;
using Microsoft.AspNetCore.Mvc;

namespace Dwapi.Exchange.Models
{
    public class DataReqDto
    {
        [FromQuery(Name = "pageNumber")] public int pageNumber { get; set; }
        [FromQuery(Name = "pageSize")] public int pageSize { get; set; }
        [FromQuery(Name = "siteCode")] public int[] siteCode { get; set; }
        [FromQuery(Name = "SiteCode")] public int[] SiteCode { get; set; }
        [FromQuery(Name = "period")] public int[] period { get; set; }
        [FromQuery(Name = "indicatorName")] public string indicatorName { get; set; }
    }
}
