using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.SharedKernel.Interfaces;
using FluentValidation;
using MediatR;
using Serilog;

namespace Dwapi.Exchange.Core.Application.Definitions.Commands
{
    public class RefreshIndex : IRequest<Result>
    {
        public string Code { get; set; }

        public RefreshIndex()
        {
        }

        public RefreshIndex(string code)
        {
            Code = code;
        }
    }

    public class RefreshIndexValidator : AbstractValidator<RefreshIndex>
    {
        public RefreshIndexValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }

    public class RefreshIndexHandler : IRequestHandler<RefreshIndex, Result>
    {
        private readonly IRegistryRepository _repository;
        private readonly IExtractReader _extractReader;

        public RefreshIndexHandler(IRegistryRepository repository, IExtractReader extractReader)
        {
            _repository = repository;
            _extractReader = extractReader;
        }

        public async Task<Result> Handle(RefreshIndex request, CancellationToken cancellationToken)
        {
            try
            {
                var registry = await _repository.GetByCode(request.Code);

                if (null == registry)
                    return Result.Success();

                foreach (var registryExtract in registry.ExtractRequests)
                {
                    registryExtract.RecordCount = await _extractReader.GetCount(registryExtract);
                    registryExtract.Refreshed = DateTime.Now;
                }

                await _repository.UpdateAsync(registry);

                return Result.Success();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(RefreshIndex)} error");
                return Result.Failure(e.Message);
            }
        }
    }
}
