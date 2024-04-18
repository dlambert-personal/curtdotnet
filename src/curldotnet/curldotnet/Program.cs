using curldotnet.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Spectre.Console.Cli;

namespace curldotnet
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {

            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .Enrich.FromLogContext()
                   .CreateLogger();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(config =>
                    {
                        config.ClearProviders();
                        config.AddProvider(new SerilogLoggerProvider(Log.Logger));
                        var minimumLevel = Configuration.GetSection("Serilog:MinimumLevel")?.Value;
                        if (!string.IsNullOrEmpty(minimumLevel))
                        {
                            config.SetMinimumLevel(Enum.Parse<LogLevel>(minimumLevel));
                        }
                    });
                });

            try
            {
                //return await builder.RunCommandLineApplicationAsync<HclCmd>(args);
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.SetApplicationName("curl-net");
                    config.ValidateExamples();
                    config.AddExample("scan-logs", @"--startdate", @"2024-1-1");

                    // Run
                    config.AddCommand<CurlCmd>("curl");
                    //config.AddCommand<ScanLogsCmd>("scan-logs");

                });
                app.SetDefaultCommand<CurlCmd>();
                return app.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}