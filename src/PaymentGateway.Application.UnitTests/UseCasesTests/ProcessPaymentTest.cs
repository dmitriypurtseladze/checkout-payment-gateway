using System;
using System.Net;
using System.Threading.Tasks;
using NSubstitute;
using PaymentGateway.Application.Abstractions;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.UseCases;
using PaymentGateway.Domain;
using PaymentGateway.Infrastructure.Abstractions;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.Application.UnitTests.UseCasesTests
{
    public class ProcessPaymentTest : BaseUseCaseTest
    {
        private readonly ProcessPaymentUseCase _subject;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankService _bankService;
        private readonly ICreditCardMasker _creditCardMasker;
        private readonly IAesHelper _aesHelper;

        public ProcessPaymentTest()
        {
            _bankService = Substitute.For<IBankService>();
            _paymentRepository = Substitute.For<IPaymentRepository>();

            _creditCardMasker = Substitute.For<ICreditCardMasker>();

            _aesHelper = Substitute.For<IAesHelper>();
            
            _subject = new ProcessPaymentUseCase(_bankService, _paymentRepository, Converter, _creditCardMasker,
                _aesHelper);
        }

        [Fact]
        public async Task ProcessPayment_ReturnsPayment()
        {
            //Arrange
            var request = new ProcessPaymentRequest
            {
                Amount = 100,
                Currency = "EUR",
                FullName = "Dmitriy Purtseladze",
                CardNumber = "5555-5555-5555-5555",
                Cvv = 123,
                Expiry = "08/22"
            };

            _bankService.ProcessPaymentAsync(request).Returns(new BankServiceResponse
            {
                IsSuccessHttpStatusCode = true,
                HttpMessage = "success",
                HttpStatusCode = HttpStatusCode.OK,
                PaymentStatus = "success",
                PaymentId = Guid.NewGuid().ToString()
            });
            const string encryptedCardNumber = "encrypted-card-number";
            const string decryptedCardNumber = "5555-5555-5555-5555";
            _aesHelper.Encrypt(decryptedCardNumber).Returns(encryptedCardNumber);

            var maskedCardNumber = "xxxx-xxxx-xxxx-5555";
            _creditCardMasker.MaskCreditCardNumber(Arg.Any<string>()).Returns(maskedCardNumber);

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Currency = request.Currency,
                FullName = request.FullName,
                CardNumber = encryptedCardNumber,
                Expiry = request.Expiry
            };
            _paymentRepository.AddAsync(Arg.Any<Payment>()).Returns(payment);
            
            var cmd = new ProcessPaymentCommand(request);

            //Act
            var response = await _subject.Handle(cmd);

            //Assert
            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(request.FullName, response.FullName);
            Assert.Equal(request.Amount, response.Amount);
            Assert.Equal(request.Currency, response.Currency);
            Assert.Equal(maskedCardNumber, response.CardNumber);
            Assert.Equal(request.Expiry, response.Expiry);

            await _bankService.Received().ProcessPaymentAsync(request);
            await _paymentRepository.Received().AddAsync(Arg.Any<Payment>());
        }
    }
}