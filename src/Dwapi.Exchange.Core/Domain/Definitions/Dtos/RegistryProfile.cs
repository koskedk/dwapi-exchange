using AutoMapper;
using Dwapi.Exchange.SharedKernel.Common;

namespace Dwapi.Exchange.Core.Domain.Definitions.Dtos
{
    public class RegistryProfile:Profile
    {
        public RegistryProfile()
        {
            CreateMap<Registry, RegistryDto>();
            CreateMap<ExtractRequest, ExtractRequestDto>();
            CreateMap<RequestFilterDto, DatasetFilter>();
        }
    }
}
