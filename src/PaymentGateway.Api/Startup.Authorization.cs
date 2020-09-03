using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Api.AuthHandlers;
using PaymentGateway.Api.Settings;

namespace PaymentGateway.Api
{
    public partial class Startup
    {
        public static void ConfigureAuth(IServiceCollection services)
        {
            ConfigureAuthPolicies(services);

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = "api";
                options.DefaultForbidScheme = "api";
                options.AddScheme<ApiSchemaHandler>("api", "api");
            });
        }

        private static void ConfigureAuthPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ApiKey, policy =>
                    policy.Requirements.Add(new ApiKeyRequirement(AuthSettings.ApiKey)));
            });
        }
    }
}