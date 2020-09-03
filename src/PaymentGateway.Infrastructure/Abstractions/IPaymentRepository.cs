using PaymentGateway.Domain;

namespace PaymentGateway.Infrastructure.Abstractions
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
    }
}