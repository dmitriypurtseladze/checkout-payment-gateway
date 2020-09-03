using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Infrastructure.Database
{
    public class DbContextOptionsFactory
    {
        public static DbContextOptions<PaymentGatewayContext> Get()
        {
            var builder = new DbContextOptionsBuilder<PaymentGatewayContext>();
            DbContextConfigurator.Configure(builder);

            return builder.Options;
        }
    }
}