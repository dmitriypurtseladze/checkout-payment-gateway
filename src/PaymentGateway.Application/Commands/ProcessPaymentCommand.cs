using System.Linq;
using System.Net;
using Backend.Common.Filters.ExceptionFilter;
using MediatR;
using PaymentGateway.Application.Validators;
using PaymentGateway.Models;

namespace PaymentGateway.Application.Commands
{
    public class ProcessPaymentCommand : IRequest<PaymentResponse>
    {
        public readonly ProcessPaymentRequest Request;

        public ProcessPaymentCommand(ProcessPaymentRequest request)
        {
            var paymentRequestValidator = new ProcessPaymentRequestValidator();
            var paymentValidationResult = paymentRequestValidator.Validate(request);
            if (!paymentValidationResult.IsValid)
            {
                throw new StatusCodeException(HttpStatusCode.BadRequest,
                    paymentValidationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            Request = request;
        }
    }
}