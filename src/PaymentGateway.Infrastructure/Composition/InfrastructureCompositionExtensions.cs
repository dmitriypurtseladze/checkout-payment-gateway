using Autofac;
using PaymentGateway.Infrastructure.Abstractions;
using PaymentGateway.Infrastructure.Database;
using PaymentGateway.Infrastructure.Repositories;
using PaymentGateway.Infrastructure.Services;

namespace PaymentGateway.Infrastructure.Composition
{
    internal static class InfrastructureCompositionExtensions
    {
        internal static ContainerBuilder AddPostgresDataProvider(this ContainerBuilder builder)
        {
            builder.RegisterType<PaymentGatewayContext>()
                .WithParameter("options", DbContextOptionsFactory.Get())
                .InstancePerLifetimeScope()
                .As<PaymentGatewayContext>();

            return builder;
        }

        internal static ContainerBuilder RegisterRepositories(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));
            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>();

            return builder;
        }
        
        internal static ContainerBuilder RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterType<BankServiceSimulator>().As<IBankService>().SingleInstance();

            return builder;
        }
    }
}