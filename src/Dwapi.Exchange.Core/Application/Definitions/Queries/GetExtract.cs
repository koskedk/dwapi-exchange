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
    public class GetExtract : IRequest<Result<PagedExtract>>
    {
        public string Code { get; }
        public string Name { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int[] SiteCode { get; }
        public string[] County { get; }
        public string Gender { get;  }
        public int Age { get;  }

        public GetExtract(string code, string name, int page, int pageSize = 50,int[] siteCode=null,string[] county=null,string gender="",int age=-1)
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

    public class GetExtractValidator : AbstractValidator<GetExtract>
    {
        public GetExtractValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class GetExtractHandler : IRequestHandler<GetExtract, Result<PagedExtract>>
    {
        private readonly IRegistryRepository _repository;
        private readonly IExtractReader _extractReader;

        public GetExtractHandler(IRegistryRepository repository, IExtractReader extractReader)
        {
            _extractReader = extractReader;
            _repository = repository;
        }

        public async Task<Result<PagedExtract>> Handle(GetExtract request, CancellationToken cancellationToken)
        {
            try
            {
                var registry = await _repository.GetByCode(request.Code);

                if (null == registry)
                    throw new Exception("Request does not exist");

                var extractRequest = registry.GetRequestByDef(request.Name);

                if (null == extractRequest)
                    throw new Exception("Request does not exist");

                PagedExtract extract;

                if (extractRequest.Name == "Profile")
                {
                    extract = await _extractReader.ReadProfileFilter(extractRequest, request.Page, request.PageSize,request.SiteCode,request.County,request.Gender,request.Age);
                }
                else
                {
                    if (AppConstants.ExtractReadMode == ReadMode.Proc)
                    {
                        extract = await _extractReader.ReadProc(extractRequest, request.Page, request.PageSize);
                    }
                    else
                    {
                        extract = await _extractReader.Read(extractRequest, request.Page, request.PageSize,
                            request.SiteCode);
                    }
                }

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
