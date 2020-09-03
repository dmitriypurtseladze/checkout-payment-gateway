using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class ProcessPaymentRequest : PaymentModel
    {
        [Required]
        public int Cvv { get; set; }
    }
}