using Autofac;
using AutofacSerilogIntegration;
using Microsoft.AspNetCore.Authorization;
using PaymentGateway.Api.AuthHandlers;
using PaymentGateway.Application.Composition;
using PaymentGateway.Infrastructure.Composition;

namespace PaymentGateway.Api.Composition
{
    internal static class ApiCompositionExtensions
    {
        public static ContainerBuilder RegisterDependentModules(this ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationCompositionRoot>();
            builder.RegisterModule<InfrastructureCompositionRoot>();
            return builder;
        }

        public static ContainerBuilder RegisterLoggingService(this ContainerBuilder builder)
        {
            builder.RegisterLogger();
            return builder;
        }

        public static ContainerBuilder RegisterAuthHandler(this ContainerBuilder builder)
        {
            builder.RegisterType<ApiKeyAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            return builder;
        }
    }
}