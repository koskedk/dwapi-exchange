using Dwapi.Exchange.Core.Domain.Definitions.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Dwapi.Exchange.Models
{
    public class FilterDto : RequestFilterDto
    {
        [FromQuery(Name = "code")] public override string code { get; set; }
        [FromQuery(Name = "name")] public override string name { get; set; }
        [FromQuery(Name = "pageNumber")] public override int pageNumber { get; set; } = 1;
        [FromQuery(Name = "pageSize")] public override int pageSize { get; set; } = 100;
        [FromQuery(Name = "siteCode")] public override int[] siteCodes { get; set; }
        [FromQuery(Name = "indicators")] public override string[] indicators { get; set; }
    }
}