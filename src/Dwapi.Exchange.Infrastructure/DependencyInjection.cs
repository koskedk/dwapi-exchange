using Dwapi.Exchange.Core.Domain.Definitions;
using Dwapi.Exchange.Infrastructure.Data;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Infrastructure.Data;
using Dwapi.Exchange.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dwapi.Exchange.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, bool initDb = true, ExtractDataSource extractDataSource = null)
        {
            var databaseType = configuration["ExtractDataSource:DatabaseType"];
            var databaseConnection = configuration["ExtractDataSource:Connection"];

            if (initDb)
            {
                services.AddDbContext<RegistryContext>(o => o.UseSqlServer(
                    configuration.GetConnectionString("LiveConnection"), x =>
                        x.MigrationsAssembly(typeof(RegistryContext).Assembly.FullName)));
                extractDataSource = ExtractDataSource.Load(databaseType, databaseConnection);
            }

            services
                .AddScoped<IRegistryRepository, RegistryRepository>()
                .AddScoped<IExtractReader>(s => new ExtractReader(extractDataSource));
            return services;
        }
    }
}
