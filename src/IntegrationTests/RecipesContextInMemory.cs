using Microsoft.EntityFrameworkCore;
using PaymentGateway.Infrastructure.Database;

namespace IntegrationTests
{
    public class PaymentGatewayContextInMemory : PaymentGatewayContext
    {
        public PaymentGatewayContextInMemory(DbContextOptions options) : base(options)
        {
        }
    }
}