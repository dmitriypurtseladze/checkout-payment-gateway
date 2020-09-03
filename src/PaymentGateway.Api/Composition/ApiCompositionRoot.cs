using Autofac;
using Microsoft.AspNetCore.Http;

namespace PaymentGateway.Api.Composition
{
    public class ApiCompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterDependentModules()
                .RegisterLoggingService()
                .RegisterAuthHandler();
            
            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>();
        }
    }
}