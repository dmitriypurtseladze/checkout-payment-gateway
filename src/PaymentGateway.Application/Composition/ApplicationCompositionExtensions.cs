using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using AutoMapper.Extensions.Autofac.DependencyInjection;
using MediatR;
using PaymentGateway.Application.Abstractions;
using PaymentGateway.Infrastructure.Composition;

namespace PaymentGateway.Application.Composition
{
    internal static class ApplicationCompositionExtensions
    {
        public static ContainerBuilder RegisterDependentModules(this ContainerBuilder builder)
        {
            builder.RegisterModule<InfrastructureCompositionRoot>();
            return builder;
        }

        internal static ContainerBuilder RegisterMapper(this ContainerBuilder builder, Assembly thisAssembly)
        {
            var profiles =
                from t in thisAssembly.GetTypes()
                where typeof(Profile).IsAssignableFrom(t)
                select (Profile) Activator.CreateInstance(t);

            builder
                .AddAutoMapper(cfg =>
                {
                    foreach (var profile in profiles)
                    {
                        cfg.AddProfile(profile);
                    }
                });

            return builder;
        }

        internal static ContainerBuilder RegisterMediator(this ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.TryResolve(t, out object o) ? o : null;
            }).InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(IUseCase).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(ApplicationCompositionRoot).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>)).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(ApplicationCompositionRoot).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>)).AsImplementedInterfaces();

            return builder;
        }

        internal static ContainerBuilder RegisterCreditCardHelpers(this ContainerBuilder builder)
        {
            builder.RegisterType<CreditCardMasker>().As<ICreditCardMasker>();
            var key = Environment.GetEnvironmentVariable("AesKey");
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            builder.RegisterType<AesHelper>().As<IAesHelper>().WithParameter("key", key);

            return builder;
        }
    }
}