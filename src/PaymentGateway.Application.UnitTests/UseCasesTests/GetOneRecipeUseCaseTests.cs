using System;
using System.Net;
using System.Threading.Tasks;
using Backend.Common.Filters.ExceptionFilter;
using NSubstitute;
using PaymentGateway.Application.Abstractions;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.UseCases;
using PaymentGateway.Domain;
using PaymentGateway.Infrastructure.Abstractions;
using Xunit;

namespace PaymentGateway.Application.UnitTests.UseCasesTests
{
    public class GetOnePaymentUseCaseTests : BaseUseCaseTest
    {
        private readonly GetOnePaymentUseCase _subject;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAesHelper _aesHelper;
        private readonly ICreditCardMasker _creditCardMasker;

        public GetOnePaymentUseCaseTests()
        {
            _paymentRepository = Substitute.For<IPaymentRepository>();
            _aesHelper = Substitute.For<IAesHelper>();
            _creditCardMasker = Substitute.For<ICreditCardMasker>();
            _subject = new GetOnePaymentUseCase(_paymentRepository, Converter, _aesHelper, _creditCardMasker);
        }

        [Fact]
        public async Task GetOnePayment_ReturnsPayment()
        {
            //Arrange
            const string encryptedCardNumber = "encrypted-card-number";
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                Currency = "EUR",
                FullName = "Dmitriy Purtseladze",
                CardNumber = encryptedCardNumber,
                Expiry = "08/22",
                BankPaymentId = Guid.NewGuid().ToString(),
                BankPaymentStatus = "success"
            };
            var query = new GetOnePaymentQuery(payment.Id);
            _paymentRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(payment);

            const string decryptedCardNumber = "5555-5555-5555-5555";

            _aesHelper.Decrypt(encryptedCardNumber).Returns(decryptedCardNumber);
            var maskedCardNumber = "xxxx-xxxx-xxxx-5555";
            
            _creditCardMasker.MaskCreditCardNumber(decryptedCardNumber).Returns(maskedCardNumber);

            //Act
            var response = await _subject.Handle(query);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(payment.Id, response.Id);
            Assert.Equal(payment.FullName, response.FullName);
            Assert.Equal(payment.Amount, response.Amount);
            Assert.Equal(payment.Currency, response.Currency);
            Assert.Equal(payment.Expiry, response.Expiry);
            Assert.Equal(payment.BankPaymentId, response.BankPaymentId);
            Assert.Equal(payment.BankPaymentStatus, response.BankPaymentStatus);

            Assert.Equal(encryptedCardNumber, payment.CardNumber);
            Assert.Equal(maskedCardNumber, response.CardNumber);
        }

        [Fact]
        public async Task GetOnePayment_ThrowsException_IfPaymentNotFound()
        {
            //Arrange
            var payment = new Payment
            {
                Id = Guid.NewGuid()
            };
            var query = new GetOnePaymentQuery(payment.Id);
            _paymentRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(Task.FromResult<Payment>(null));

            //Act
            //Assert
            var exception = await Assert.ThrowsAsync<StatusCodeException>(async () => await _subject.Handle(query));
            Assert.Equal(HttpStatusCode.NotFound, exception.HttpStatusCode);
        }
    }
}