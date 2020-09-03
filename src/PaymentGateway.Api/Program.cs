using System;
using System.Threading.Tasks;
using App.Metrics.AspNetCore;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Infrastructure.Database;
using Serilog;

namespace PaymentGateway.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await MigrateDatabase(host);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .UseMetrics()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(
                        $"http://{Environment.GetEnvironmentVariable("HOST")}:{Environment.GetEnvironmentVariable("PORT")}/");
                });

        private static async Task MigrateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<PaymentGatewayContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.ToString(), "An error occurred migrating the DB.");
            }
        }
    }
}