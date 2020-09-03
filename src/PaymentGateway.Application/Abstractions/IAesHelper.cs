namespace PaymentGateway.Application.Abstractions
{
    public interface IAesHelper
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}