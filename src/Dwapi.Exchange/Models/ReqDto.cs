using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Dwapi.Exchange.Models
{
    public class ReqDto
    {
        [FromQuery(Name = "code")] public string code { get; set; }
        [FromQuery(Name = "pageNumber")] public int pageNumber { get; set; }
        [FromQuery(Name = "pageSize")] public int pageSize { get; set; }
        [FromQuery(Name = "siteCode")] public int[] siteCode { get; set; }
        [FromQuery(Name = "county")] public string[] county { get; set; }
    }
}
