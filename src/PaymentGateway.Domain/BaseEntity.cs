using System;

namespace PaymentGateway.Domain
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}