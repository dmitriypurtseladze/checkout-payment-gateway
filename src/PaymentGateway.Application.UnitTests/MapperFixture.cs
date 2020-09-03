using System;
using System.Linq;
using AutoMapper;
using PaymentGateway.Application.Mapping;

namespace PaymentGateway.Application.UnitTests
{
    public class MapperFixture
    {
        public IMapper CreateMapper()
        {
            var profiles =
                from t in typeof(PaymentProfile).Assembly.GetTypes()
                where typeof(Profile).IsAssignableFrom(t)
                select (Profile) Activator.CreateInstance(t);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            return new Mapper(config);
        }
    }
}