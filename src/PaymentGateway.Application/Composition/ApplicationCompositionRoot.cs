using Autofac;

namespace PaymentGateway.Application.Composition
{
    public class ApplicationCompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterDependentModules()
                .RegisterMapper(ThisAssembly)
                .RegisterMediator()
                .RegisterCreditCardHelpers();
        }
    }
}