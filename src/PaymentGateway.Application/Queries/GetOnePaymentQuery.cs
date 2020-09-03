using System;
using MediatR;
using PaymentGateway.Models;

namespace PaymentGateway.Application.Queries
{
    public class GetOnePaymentQuery : IRequest<PaymentResponse>
    {
        public readonly Guid PaymentId;

        public GetOnePaymentQuery(Guid paymentId)
        {
            PaymentId = paymentId;
        }
    }
}