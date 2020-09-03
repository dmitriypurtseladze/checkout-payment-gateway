using Microsoft.EntityFrameworkCore;
using PaymentGateway.Infrastructure.Settings;

namespace PaymentGateway.Infrastructure.Database
{
    public class DbContextConfigurator
    {
        public static void Configure(DbContextOptionsBuilder<PaymentGatewayContext> builder)
        {
            var connectionString = ConnectionSettings.PostgresConnection;

            builder
                .UseNpgsql(connectionString,
                    pg => pg.MigrationsAssembly("PaymentGateway.Infrastructure"));
        }
    }
}