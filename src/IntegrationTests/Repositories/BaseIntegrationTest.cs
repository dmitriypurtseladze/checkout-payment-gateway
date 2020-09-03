using PaymentGateway.Infrastructure.Repositories;
using Xunit;

namespace IntegrationTests.Repositories
{
    public class BaseIntegrationTest : IClassFixture<DatabaseFixture>
    {
        protected BaseIntegrationTest()
        {
            var databaseFixture = new DatabaseFixture();
            PaymentRepository = databaseFixture.PaymentRepository;
            PaymentGatewayContextInMemory = databaseFixture.PaymentGatewayContextInMemory;
        }

        protected PaymentRepository PaymentRepository { get; set; }
        
        public PaymentGatewayContextInMemory PaymentGatewayContextInMemory { get; set; }
    }
}