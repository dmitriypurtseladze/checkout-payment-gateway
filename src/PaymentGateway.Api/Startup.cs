using System;
using System.IO;
using System.Reflection;
using Autofac;
using Backend.Common;
using Backend.Common.Filters.ExceptionFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PaymentGateway.Api.AuthHandlers;
using PaymentGateway.Api.Composition;
using PaymentGateway.Api.Settings;
using PaymentGateway.Infrastructure.Settings;
using Serilog;

namespace PaymentGateway.Api
{
    public partial class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IConfiguration Configuration { get; }

        private string ApiVersion { get; }

        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _webHostEnvironment = webHostEnvironment;
            Configuration = config;

            Configuration.GetSection("ApiVersionSettings").Bind(new ApiVersionSettings());
            ApiVersion = $"v{ApiVersionSettings.MajorVersion}.{ApiVersionSettings.MinorVersion}";

            Configuration.GetSection("ConnectionSettings").Bind(new ConnectionSettings());
            Configuration.GetSection("AuthSettings").Bind(new AuthSettings());

            var logger = AddLogging();
            Log.Logger = logger.CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddMetrics();
            
            services.AddControllers();

            services.AddMvcCore(options => { options.Filters.Add(new ExceptionFilter()); });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(ApiVersionSettings.MajorVersion, ApiVersionSettings.MinorVersion);
                o.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, new OpenApiInfo
                {
                    Version = ApiVersion,
                    Title = ApiVersionSettings.Title
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<HeaderFilter>();
            });

            ConfigureAuth(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var allowedCors = Configuration.GetSection("AllowedHosts")?.Get<string[]>();
            app.UseCorsUrls(allowedCors);
            
            app.UseHealthChecks("/health", new HealthCheckOptions {Predicate = check => check.Tags.Contains("ready")});

            app.Map("/ping", PingEndpoint);

            app.Use((context, next) =>
            {
                if (!env.IsDevelopment())
                {
                    context.Request.Scheme = "https";
                }

                return next();
            });

            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{ApiVersion}/swagger.json",
                        ApiVersionSettings.Title);
                });
            }
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ApiCompositionRoot>();
        }
        
        private static void PingEndpoint(IApplicationBuilder app)
        {
            app.Run(async context => { await context.Response.WriteAsync("pong"); });
        }
    }
}