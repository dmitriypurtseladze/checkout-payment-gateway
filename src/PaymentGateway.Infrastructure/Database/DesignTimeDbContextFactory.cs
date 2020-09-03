using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PaymentGateway.Infrastructure.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PaymentGatewayContext>
    {
        public PaymentGatewayContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PaymentGatewayContext>();
            DbContextConfigurator.Configure(builder);

            return new PaymentGatewayContext(builder.Options);
        }
    }
}