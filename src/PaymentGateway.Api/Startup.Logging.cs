using System;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Api.Settings;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace PaymentGateway.Api
{
    public partial class Startup
    {
        private LoggerConfiguration AddLogging()
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment",
                    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.MachineName)
                .Enrich.WithProperty("Server", Environment.MachineName)
                .Enrich.WithProperty("Application", ApiVersionSettings.Title);

            if (_webHostEnvironment.IsDevelopment())
            {
                logger.WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Code);
            }
            else
            {
                logger.WriteTo.Console(new CompactJsonFormatter());
            }

            return logger;
        }
    }
}