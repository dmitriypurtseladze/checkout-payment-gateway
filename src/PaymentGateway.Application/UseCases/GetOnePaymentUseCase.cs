using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Common.Filters.ExceptionFilter;
using MediatR;
using PaymentGateway.Application.Abstractions;
using PaymentGateway.Application.Queries;
using PaymentGateway.Infrastructure.Abstractions;
using PaymentGateway.Models;

namespace PaymentGateway.Application.UseCases
{
    public class GetOnePaymentUseCase : IUseCase, IRequestHandler<GetOnePaymentQuery, PaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _converter;
        private readonly IAesHelper _aesHelper;
        private readonly ICreditCardMasker _creditCardMasker;

        public GetOnePaymentUseCase(
            IPaymentRepository paymentRepository,
            IMapper converter,
            IAesHelper aesHelper,
            ICreditCardMasker creditCardMasker
        )
        {
            _paymentRepository = paymentRepository;
            _converter = converter;
            _aesHelper = aesHelper;
            _creditCardMasker = creditCardMasker;
        }

        public async Task<PaymentResponse> Handle(GetOnePaymentQuery query,
            CancellationToken cancellationToken = default)
        {
            var payment = await _paymentRepository.GetByIdAsync(query.PaymentId);
            if (payment == null)
            {
                throw new StatusCodeException(HttpStatusCode.NotFound, "Could not find the requested item.");
            }

            var decryptedCardNumber = _aesHelper.Decrypt(payment.CardNumber);
            var response = _converter.Map<PaymentResponse>(payment);
            response.CardNumber = _creditCardMasker.MaskCreditCardNumber(decryptedCardNumber);
            return response;
        }
    }
}