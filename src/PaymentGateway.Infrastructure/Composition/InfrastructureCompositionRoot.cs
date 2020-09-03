using Autofac;

namespace PaymentGateway.Infrastructure.Composition
{
    public class InfrastructureCompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .AddPostgresDataProvider()
                .RegisterRepositories()
                .RegisterServices();
        }
    }
}