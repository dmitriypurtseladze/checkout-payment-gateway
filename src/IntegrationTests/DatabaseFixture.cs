using Microsoft.EntityFrameworkCore;
using PaymentGateway.Infrastructure.Repositories;

namespace IntegrationTests
{
    public class DatabaseFixture
    {
        public PaymentRepository PaymentRepository { get; set; }
        public PaymentGatewayContextInMemory PaymentGatewayContextInMemory { get; set; }

        public DatabaseFixture()
        {
            var dbOptions = new DbContextOptionsBuilder<PaymentGatewayContextInMemory>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            PaymentGatewayContextInMemory = new PaymentGatewayContextInMemory(dbOptions);

            PaymentRepository = new PaymentRepository(PaymentGatewayContextInMemory);
        }
    }
}