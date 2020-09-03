using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;

namespace Backend.Common
{
    public static class CorsBuilder
    {
        public static void UseCorsUrls(this IApplicationBuilder app, string[] allowedCors)
        {
            app.UseCors(ConfigureCorsPolicy(allowedCors));
        }

        private static Action<CorsPolicyBuilder> ConfigureCorsPolicy(string[] allowedCors)
        {
            return builder =>
            {
                if (allowedCors == null)
                {
                    Log.Warning("Cors policy is not configured");
                    return;
                }

                if (allowedCors.ToList().Contains("*"))
                {
                    builder.AllowAnyOrigin();
                }
                else
                {
                    builder.WithOrigins(allowedCors);
                }

                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            };
        }
    }
}