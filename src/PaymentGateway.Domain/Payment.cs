namespace PaymentGateway.Domain
{
    public class Payment : BaseEntity
    {
        public string CardNumber { get; set; }

        public string Expiry { get; set; }

        public string FullName { get; set; }

        public float Amount { get; set; }

        public string Currency { get; set; }

        public string BankPaymentId { get; set; }

        public string BankPaymentStatus { get; set; }
    }
}