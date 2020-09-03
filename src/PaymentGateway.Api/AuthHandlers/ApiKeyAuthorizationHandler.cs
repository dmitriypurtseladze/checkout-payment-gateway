using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace PaymentGateway.Api.AuthHandlers
{
    public class ApiKeyAuthorizationHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiKeyAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static bool IsContainsCorrectApiKey(IHttpContextAccessor httpContextAccessor, string apiKey)
        {
            var headers = httpContextAccessor.HttpContext.Request.Headers;
            var result = headers.ContainsKey("x-api-key") && headers["x-api-key"] == apiKey;
            return result;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ApiKeyRequirement requirement)
        {
            if (IsContainsCorrectApiKey(_httpContextAccessor, requirement.ApiKey))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}