using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Dwapi.Exchange.Core.Application.Common.Behaviors;
using Dwapi.Exchange.Core.Application.Definitions.Queries;
using Dwapi.Exchange.Core.Domain.Definitions.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Dwapi.Exchange.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, List<Assembly> mediatrAssemblies = null, List<Assembly> mapperAssemblies = null)
        {
            if (null != mapperAssemblies)
            {
                mapperAssemblies.Add(typeof(RegistryProfile).Assembly);
                services.AddAutoMapper(mediatrAssemblies.ToArray());
            }
            else
            {
                services.AddAutoMapper(typeof(RegistryProfile));
            }
            
            if (null != mediatrAssemblies)
            {
                mediatrAssemblies.Add(typeof(GetExtract).Assembly);
                services.AddMediatR(mediatrAssemblies.ToArray());
            }
            else
            {
                services.AddMediatR(typeof(GetExtract).Assembly);
            }
            services.AddValidatorsFromAssemblyContaining<GetExtract>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
