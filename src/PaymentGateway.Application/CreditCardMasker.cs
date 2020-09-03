using PaymentGateway.Application.Abstractions;

namespace PaymentGateway.Application
{
    public class CreditCardMasker : ICreditCardMasker
    {
        public string MaskCreditCardNumber(string creditCardNumber)
        {
            var number = creditCardNumber.Trim();
            var result = $"xxxx-xxxx-xxxx-{number.Substring(number.Length - 4, 4)}";
            return result;
        }
    }
}