using System;
using System.Net;
using System.Threading.Tasks;
using Backend.Common.Filters.ExceptionFilter;
using MediatR;
using NSubstitute;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.Api.UnitTests.Controllers
{
    public class PaymentsControllerTests
    {
        protected readonly IMediator Mediator;
        private readonly PaymentsController _subject;
        private readonly Guid _paymentId;

        public PaymentsControllerTests()
        {
            Mediator = Substitute.For<IMediator>();
            _subject = new PaymentsController(Mediator);
            _paymentId = Guid.NewGuid();
        }

        [Fact]
        public async Task CreatePaymentAsync()
        {
            //Arrange
            var payment = new ProcessPaymentRequest
            {
                Amount = 100,
                Currency = "EUR",
                FullName = "Dmitriy Purtseladze",
                CardNumber = "5555-5555-5555-5555",
                Cvv = 123,
                Expiry = "08/22"
            };

            //Act
            await _subject.ProcessPaymentAsync(payment);

            //Assert
            await Mediator.Received().Send(Arg.Any<ProcessPaymentCommand>());
        }

        [Fact]
        public async Task ProcessPaymentAsync_ReturnsBadRequest_WhenAnyDataIsEmpty()
        {
            //Arrange
            var payment = new ProcessPaymentRequest
            {
                Amount = 100,
                Currency = string.Empty,
                FullName = "Dmitriy Purtseladze",
                CardNumber = string.Empty,
                Cvv = 123,
                Expiry = "08/22"
            };

            //Act
            //Assert
            var exception =
                await Assert.ThrowsAsync<StatusCodeException>(async () => await _subject.ProcessPaymentAsync(payment));
            Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ReturnsBadRequest_WhenAmountIsZero()
        {
            //Arrange
            var payment = new ProcessPaymentRequest
            {
                Amount = 0,
                Currency = "EUR",
                FullName = "Dmitriy Purtseladze",
                CardNumber = "5555-5555-5555-5555",
                Cvv = 123,
                Expiry = "08/22"
            };

            //Act
            //Assert
            var exception =
                await Assert.ThrowsAsync<StatusCodeException>(async () => await _subject.ProcessPaymentAsync(payment));
            Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
        }


        [Fact]
        public async Task GetOnePaymentAsync()
        {
            //Arrange
            //Act
            await _subject.GetOnePaymentAsync(_paymentId);

            //Assert
            await Mediator.Received().Send(Arg.Any<GetOnePaymentQuery>());
        }
    }
}