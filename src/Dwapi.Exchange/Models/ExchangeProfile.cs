using AutoMapper;
using Dwapi.Exchange.Core.Domain.Definitions.Dtos;

namespace Dwapi.Exchange.Models
{
    public class ExchangeProfile:Profile
    {
        public ExchangeProfile()
        {
            CreateMap<EmrReqDto, EmrRequestDto>();
        }
    }
}
