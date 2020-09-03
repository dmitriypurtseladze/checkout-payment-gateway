using System;
using System.Threading.Tasks;
using IntegrationTests.Orderers;
using PaymentGateway.Domain;
using Xunit;

namespace IntegrationTests.Repositories
{
    [TestCaseOrderer("IntegrationTests.Orderers.PriorityOrderer", "IntegrationTests")]
    public class PaymentsRepositoryIntegrationTests : BaseIntegrationTest
    {
        private static Guid _id;
        private const string CreatedFullName = "Dmitriy Purtseladze";

        [Fact, TestPriority(0)]
        public async Task AddPaymentAsync_RunsOk()
        {
            //Arrange
            var paymentToCreate = new Payment
            {
                FullName = CreatedFullName,
                Amount = 100,
                Currency = "EUR",
                CardNumber = "encrypted-card-number",
                Expiry = "08/22",
                BankPaymentId = Guid.NewGuid().ToString(),
                BankPaymentStatus = "success"
            };

            //Act
            var result = await PaymentRepository.AddAsync(paymentToCreate);
            _id = result.Id;

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, _id);
            Assert.Equal(CreatedFullName, result.FullName);
            Assert.Equal(paymentToCreate.Amount, result.Amount);
            Assert.Equal(paymentToCreate.Currency, result.Currency);
            Assert.Equal(paymentToCreate.CardNumber, result.CardNumber);
            Assert.Equal(paymentToCreate.Expiry, result.Expiry);
            Assert.Equal(paymentToCreate.BankPaymentId, result.BankPaymentId);
            Assert.Equal(paymentToCreate.BankPaymentStatus, result.BankPaymentStatus);
            Assert.False(result.CreatedAt == DateTime.MinValue);
        }

        [Fact, TestPriority(1)]
        public async Task PaymentGetByIdAsync_RunsOk()
        {
            //Arrange
            //Act
            var result = await PaymentRepository.GetByIdAsync(_id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_id, result.Id);
            Assert.Equal(CreatedFullName, result.FullName);
        }
    }
}