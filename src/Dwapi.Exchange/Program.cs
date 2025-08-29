using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Dwapi.Exchange
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfig(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information($"Starting Dwapi.Exchange...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(GetConfig(args));
                    webBuilder.UseStartup<Startup>();
                });

        private static IConfigurationRoot GetConfig(string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("hosting.json", optional: true)
                .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args).Build();
        }
    }
}
