using AutoMapper;
using Dwapi.Exchange.Models;

namespace Dwapi.Exchange.Core.Domain.Definitions.Dtos
{
    public class ExchangeProfile:Profile
    {
        public ExchangeProfile()
        {
            CreateMap<EmrReqDto, EmrRequestDto>();
        }
    }
}
