using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class PaymentModel
    {
        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string Expiry { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public string Currency { get; set; }
    }
}