using FluentValidation;
using PaymentGateway.Models;

namespace PaymentGateway.Application.Validators
{
    public class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
    {
        public ProcessPaymentRequestValidator()
        {
            RuleFor(x => x.Cvv).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.CardNumber).NotEmpty();
            RuleFor(x => x.Expiry).NotEmpty();
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty();
        }
    }
}