using System;

namespace PaymentGateway.Models
{
    public class PaymentResponse : PaymentModel
    {
        public Guid Id { get; set; }

        public DateTime PaymentDateTime { get; set; }

        public string BankPaymentStatus { get; set; }

        public string BankPaymentId { get; set; }
    }
}