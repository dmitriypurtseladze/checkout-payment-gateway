using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PaymentGateway.Application.Abstractions;
using PaymentGateway.Application.Commands;
using PaymentGateway.Domain;
using PaymentGateway.Infrastructure.Abstractions;
using PaymentGateway.Models;

namespace PaymentGateway.Application.UseCases
{
    public class ProcessPaymentUseCase : IUseCase, IRequestHandler<ProcessPaymentCommand, PaymentResponse>
    {
        private readonly IBankService _bankService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _converter;
        private readonly ICreditCardMasker _creditCardMasker;
        private readonly IAesHelper _aesHelper;

        public ProcessPaymentUseCase(
            IBankService bankService,
            IPaymentRepository paymentRepository,
            IMapper converter,
            ICreditCardMasker creditCardMasker,
            IAesHelper aesHelper)
        {
            _bankService = bankService;
            _paymentRepository = paymentRepository;
            _converter = converter;
            _creditCardMasker = creditCardMasker;
            _aesHelper = aesHelper;
        }

        public async Task<PaymentResponse> Handle(ProcessPaymentCommand command,
            CancellationToken cancellationToken = default)
        {
            var bankResponse = await _bankService.ProcessPaymentAsync(command.Request);

            var paymentToSave = _converter.Map<Payment>(command.Request);
            paymentToSave.CardNumber = _aesHelper.Encrypt(paymentToSave.CardNumber);
            paymentToSave.BankPaymentStatus = bankResponse.PaymentStatus;
            paymentToSave.BankPaymentId = bankResponse.PaymentId;

            var payment = await _paymentRepository.AddAsync(paymentToSave);

            var paymentResponse = _converter.Map<PaymentResponse>(payment);
            paymentResponse.CardNumber = _creditCardMasker.MaskCreditCardNumber(command.Request.CardNumber);
            return paymentResponse;
        }
    }
}