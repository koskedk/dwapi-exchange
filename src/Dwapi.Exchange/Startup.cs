using System;
using System.Collections.Generic;
using System.Reflection;
using Dwapi.Exchange.Core;
using Dwapi.Exchange.Core.Domain.Definitions.Dtos;
using Dwapi.Exchange.Infrastructure;
using Dwapi.Exchange.Infrastructure.Data;
using Dwapi.Exchange.Models;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Dwapi.Exchange
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public bool DisableAuth { get; set; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authority = Configuration["Authority"];
            DisableAuth = string.IsNullOrWhiteSpace(authority);
            try
            {
                AppConstants.ExtractReadMode = (ReadMode) Convert.ToInt32(Configuration["ReadMode"]);
            }
            catch (Exception e)
            {
                Log.Warning("ReadMode setting could not be SET");
            }

            IdentityModelEventSource.ShowPII = true;

            if (DisableAuth)
                services.AddControllers();
            else
               services.AddControllers(opt => opt.Filters.Add(new AuthorizeFilter()));

                ;//.AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; } );
            services.AddSwaggerGen();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authority;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddInfrastructure(Configuration);
            services.AddApplication(null, new List<Assembly> { typeof(ExchangeProfile).Assembly });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dwapi.Exchange V1");
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            EnsureMigrationOfContext<RegistryContext>(app);
            Dapper.SqlMapper.Settings.CommandTimeout = 3600;
            Log.Information($"Dwapi.Exchange [Version {GetType().Assembly.GetName().Version}] started successfully");
        }

        private static void EnsureMigrationOfContext<T>(IApplicationBuilder app) where T : BaseContext
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contextName = typeof(T).Name;
                Log.Debug($"initializing Database context: {contextName}");
                var context = serviceScope.ServiceProvider.GetService<T>();
                try
                {
                    context.Database.Migrate();
                    context.EnsureSeeded();
                    Log.Debug($"initializing Database context: {contextName} [OK]");
                }
                catch (Exception e)
                {
                    var msg = $"initializing Database context: {contextName} Error";
                    Log.Error(e, msg);
                }
            }
        }
    }
}
