using PaymentGateway.Domain;
using PaymentGateway.Infrastructure.Abstractions;
using PaymentGateway.Infrastructure.Database;

namespace PaymentGateway.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly PaymentGatewayContext _dbContext;

        public PaymentRepository(PaymentGatewayContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}