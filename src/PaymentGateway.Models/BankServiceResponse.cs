using System.Net;

namespace PaymentGateway.Models
{
    public class BankServiceResponse
    {
        public bool IsSuccessHttpStatusCode { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpMessage { get; set; }
        
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
    }
}