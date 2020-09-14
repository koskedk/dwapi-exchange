using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Custom;
using Dwapi.Exchange.SharedKernel.Infrastructure.Data;
using Dwapi.Exchange.SharedKernel.Infrastructure.Tests.TestArtifacts;
using Dwapi.Exchange.SharedKernel.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;
        public static IServiceCollection Services;
        public static IServiceCollection ServicesOnly;
        public static string LiveConnectionString;
        public static string ExtractsConnectionString;
        public static IConfigurationRoot Configuration;
        public static ExtractDataSource ExtractDataSource;

        [OneTimeSetUp]
        public void Init()
        {
            RemoveTestsFilesDbs();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var dir = $"{TestContext.CurrentContext.TestDirectory.HasToEndWith(@"/")}";

            var config = Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var connectionString = config.GetConnectionString("LiveConnection")
                .Replace("#dir#", dir);
            LiveConnectionString = connectionString.ToOsStyle();
            
            var databaseType = Configuration["ExtractDataSource:DatabaseType"];
            var databaseConnection = Configuration["ExtractDataSource:Connection"];
            ExtractDataSource = ExtractDataSource.Load(databaseType, databaseConnection);

            var extractConnection = ExtractDataSource.Connection.Replace("#dir#", dir);
            ExtractsConnectionString = extractConnection.ToOsStyle();

        
            var liveConnection = new SqliteConnection(connectionString.Replace(".db", $"{DateTime.Now.Ticks}.db"));
            liveConnection.Open();

            var services = new ServiceCollection().AddDbContext<TestDbContext>(x => x.UseSqlite(liveConnection));

            services
                .AddTransient<TestDbContext>()
                .AddTransient<ITestCarRepository, TestCarRepository>()
                .AddTransient<IExtractReader>(s =>
                    new ExtractReader(new ExtractDataSource(DatabaseType.SqLite, extractConnection)));

            Services = services;

            ServicesOnly = Services;
            ServiceProvider = Services.BuildServiceProvider();
        }

        public static void ClearDb()
        {
            var context = ServiceProvider.GetService<TestDbContext>();
            context.Database.EnsureCreated();
        }
        public static void SeedData(params IEnumerable<object>[] entities)
        {
            var context = ServiceProvider.GetService<TestDbContext>();
            foreach (IEnumerable<object> t in entities)
            {
                context.AddRange(t);
            }
            context.SaveChanges();
        }

        private void RemoveTestsFilesDbs()
        {
            string[] keyFiles =
                { "demo.db","source.db"};
            string[] keyDirs = { @"TestArtifacts/Database".ToOsStyle()};

            foreach (var keyDir in keyDirs)
            {
                DirectoryInfo di = new DirectoryInfo(keyDir);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (!keyFiles.Contains(file.Name))
                        file.Delete();
                }
            }
        }
    }
}
