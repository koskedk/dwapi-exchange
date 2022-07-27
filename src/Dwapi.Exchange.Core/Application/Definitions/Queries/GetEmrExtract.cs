using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Core.Domain.Definitions.Dtos;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Interfaces;
using FluentValidation;
using MediatR;
using Serilog;

namespace Dwapi.Exchange.Core.Application.Definitions.Queries
{
    public class GetEmrExtract : IRequest<Result<PagedExtract>>
    {
        public EmrRequestDto EmrRequestDto { get; }

        public GetEmrExtract(EmrRequestDto emrRequest)
        {
            EmrRequestDto = emrRequest;
        }
    }

    public class GetEmrExtractValidator : AbstractValidator<GetEmrExtract>
    {
        public GetEmrExtractValidator()
        {
            RuleFor(x => x.EmrRequestDto.Code).NotEmpty();
            RuleFor(x => x.EmrRequestDto.Name).NotEmpty();
            RuleFor(x => x.EmrRequestDto.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.EmrRequestDto.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class GetEmrExtractHandler : IRequestHandler<GetEmrExtract, Result<PagedExtract>>
    {
        private readonly IRegistryRepository _repository;
        private readonly IExtractReader _extractReader;

        public GetEmrExtractHandler(IRegistryRepository repository, IExtractReader extractReader)
        {
            _extractReader = extractReader;
            _repository = repository;
        }

        public async Task<Result<PagedExtract>> Handle(GetEmrExtract request, CancellationToken cancellationToken)
        {
            try
            {
                var registry = await _repository.GetByCode(request.EmrRequestDto.Code);

                if (null == registry)
                    throw new Exception("Request does not exist");

                var extractRequest = registry.GetRequestByDef(request.EmrRequestDto.Name);

                if (null == extractRequest)
                    throw new Exception("Request does not exist");

                PagedExtract extract;
                
                extract = await _extractReader.Read(extractRequest, request.EmrRequestDto.PageNumber, request.EmrRequestDto.PageSize,
                    request.EmrRequestDto.EvaluationDate,request.EmrRequestDto.SiteCode,request.EmrRequestDto.CccNumber);

                return Result.Success(extract);
            }
            catch (Exception e)
            {
                Log.Error(e, "GetExtract error");
                return Result.Failure<PagedExtract>(e.Message);
            }
        }
    }
}
