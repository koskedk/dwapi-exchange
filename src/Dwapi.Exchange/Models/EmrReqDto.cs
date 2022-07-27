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
        [FromQuery(Name = "fromDate")] public DateTime? fromDate { get; set; }
        [FromQuery(Name = "toDate")] public DateTime? toDate { get; set; }
        [FromQuery(Name = "cccNumber")] public string cccNumber { get; set; }
    }
}