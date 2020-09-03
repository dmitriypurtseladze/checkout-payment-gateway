using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Infrastructure.Abstractions
{
    public interface IBankService
    {
         Task<BankServiceResponse> ProcessPaymentAsync(ProcessPaymentRequest request);
    }
}