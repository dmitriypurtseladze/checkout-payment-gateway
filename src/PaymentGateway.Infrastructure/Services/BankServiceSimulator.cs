using System;
using System.Net;
using System.Threading.Tasks;
using PaymentGateway.Infrastructure.Abstractions;
using PaymentGateway.Models;

namespace PaymentGateway.Infrastructure.Services
{
    public class BankServiceSimulator : IBankService
    {
        private readonly Random _random;

        public BankServiceSimulator()
        {
            _random = new Random();
        }

        public async Task<BankServiceResponse> ProcessPaymentAsync(ProcessPaymentRequest request)
        {
            var response = new BankServiceResponse
            {
                PaymentId = Guid.NewGuid().ToString()
            };
            var value = _random.Next(0, 2);
            if (value == 0)
            {
                response.HttpStatusCode = HttpStatusCode.InternalServerError;
                response.HttpMessage = "bank service return an error";
                response.PaymentStatus = "expired";
            }
            else
            {
                response.HttpStatusCode = HttpStatusCode.OK;
                response.HttpMessage = "success";
                response.IsSuccessHttpStatusCode = true;
                response.PaymentStatus = "success";
            }

            return await Task.FromResult(response);
        }
    }
}