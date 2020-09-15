using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Core.Domain.Definitions.Dtos;
using MediatR;
using Serilog;

namespace Dwapi.Exchange.Core.Application.Definitions.Queries
{
    public class GetRegistry : IRequest<Result<List<RegistryDto>>>
    {
    }

    public class GetRegistryHandler : IRequestHandler<GetRegistry, Result<List<RegistryDto>>>
    {
        private readonly IRegistryRepository _repository;
        private readonly IMapper _mapper;

        public GetRegistryHandler(IRegistryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<Result<List<RegistryDto>>> Handle(GetRegistry request, CancellationToken cancellationToken)
        {
            try
            {
                var registry =  _repository.GetAll().ToList();
                return Task.FromResult(Result.Success(_mapper.Map<List<RegistryDto>>(registry)));
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(GetExtract)} error");
                return Task.FromResult(Result.Failure<List<RegistryDto>>(e.Message));
            }
        }
    }
}
