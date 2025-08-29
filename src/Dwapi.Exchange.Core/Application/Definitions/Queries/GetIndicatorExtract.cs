using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Interfaces;
using FluentValidation;
using MediatR;
using Serilog;

namespace Dwapi.Exchange.Core.Application.Definitions.Queries
{
    public class GetIndicatorExtract : IRequest<Result<PagedExtract>>
    {
        public DatasetFilter Filter {get; set;}

        public GetIndicatorExtract(DatasetFilter filter)
        {
            Filter = filter;
        }
    }

    public class GetIndicatorExtractValidator : AbstractValidator<GetIndicatorExtract>
    {
        public GetIndicatorExtractValidator()
        {
            RuleFor(x => x.Filter.Code).NotEmpty();
            RuleFor(x => x.Filter.Name).NotEmpty();
            RuleFor(x => x.Filter.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Filter.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class GetIndicatorExtractHandler : IRequestHandler<GetIndicatorExtract, Result<PagedExtract>>
    {
        private readonly IRegistryRepository _repository;
        private readonly IExtractReader _extractReader;

        public GetIndicatorExtractHandler(IRegistryRepository repository, IExtractReader extractReader)
        {
            _extractReader = extractReader;
            _repository = repository;
        }

        public async Task<Result<PagedExtract>> Handle(GetIndicatorExtract request, CancellationToken cancellationToken)
        {
            try
            {
                var registry = await _repository.GetByCode(request.Filter.Code);

                if (null == registry)
                    throw new Exception("Request does not exist");

                var extractRequest = registry.GetRequestByDef(request.Filter.Name);

                if (null == extractRequest)
                    throw new Exception("Request does not exist");

                var extract = await _extractReader.Read(extractRequest, request.Filter);

                return Result.Success(extract);
            }
            catch (Exception e)
            {
                Log.Error(e, "GetIndicatorExtract error");
                return Result.Failure<PagedExtract>(e.Message);
            }
        }
    }


}
