using Microsoft.AspNetCore.Authorization;

namespace PaymentGateway.Api.AuthHandlers
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public string ApiKey { get; private set; }

        public ApiKeyRequirement(string apiKey)
        {
            ApiKey = apiKey;
        }
    }
}
