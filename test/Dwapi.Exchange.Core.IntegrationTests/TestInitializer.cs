using System;
using System.Collections.Generic;
using System.Reflection;
using Dwapi.Exchange.Infrastructure;
using Dwapi.Exchange.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.Core.IntegrationTests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;
        public static IConfigurationRoot Configuration;

        [OneTimeSetUp]
        public void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var config = Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddInfrastructure(Configuration);
            services.AddApplication(null, new List<Assembly> { typeof(ExchangeProfile).Assembly });
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
