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
    public class GetProfileExtract : IRequest<Result<PagedProfileExtract>>
    {
        public string Code { get; }
        public string Name { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int[] SiteCode { get; }
        public string[] County { get; }
        public string Gender { get;  }
        public int Age { get;  }

        public GetProfileExtract(string code, string name, int page, int pageSize = 50,int[] siteCode=null,string[] county=null,string gender="",int age=-1)
        {
            Code = code;
            Name = name;
            Page = page;
            PageSize = pageSize;
            SiteCode = siteCode;
            County = county;
            Gender = gender;
            Age = age;
        }
    }

    public class GetProfileExtractValidator : AbstractValidator<GetProfileExtract>
    {
        public GetProfileExtractValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class GetProfileExtractHandler : IRequestHandler<GetProfileExtract, Result<PagedProfileExtract>>
    {
        private readonly IRegistryRepository _repository;
        private readonly IExtractReader _extractReader;

        public GetProfileExtractHandler(IRegistryRepository repository, IExtractReader extractReader)
        {
            _extractReader = extractReader;
            _repository = repository;
        }

        public async Task<Result<PagedProfileExtract>> Handle(GetProfileExtract request, CancellationToken cancellationToken)
        {
            try
            {
                var registry = await _repository.GetByCode(request.Code);

                if (null == registry)
                    throw new Exception("Request does not exist");

                var mainExtractRequest = registry.GetMainRequest();

                if (null == mainExtractRequest)
                    throw new Exception("Main Request does not exist");

                var extractRequest = registry.GetRequestByDef(request.Name);

                if (null == extractRequest)
                    throw new Exception("Request does not exist");

                PagedProfileExtract extract;

                extract = await _extractReader.ReadProfileFilterExpress(mainExtractRequest,extractRequest, request.Page, request.PageSize,request.SiteCode,request.County,request.Gender,request.Age);

                return Result.Success(extract);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(GetExtract)} error");
                return Result.Failure<PagedProfileExtract>(e.Message);
            }
        }
    }


}
