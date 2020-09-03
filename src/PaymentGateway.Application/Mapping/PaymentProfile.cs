using AutoMapper;
using PaymentGateway.Domain;
using PaymentGateway.Models;

namespace PaymentGateway.Application.Mapping
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<ProcessPaymentRequest, Payment>();
            CreateMap<Payment, PaymentResponse>()
                .ForMember(dest => dest.PaymentDateTime, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}