namespace PaymentGateway.Application.Abstractions
{
    public interface ICreditCardMasker
    {
        string MaskCreditCardNumber(string creditCardNumber);
    }
}