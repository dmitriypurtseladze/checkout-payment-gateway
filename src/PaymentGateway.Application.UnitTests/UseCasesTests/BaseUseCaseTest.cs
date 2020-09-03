using AutoMapper;
using Xunit;

namespace PaymentGateway.Application.UnitTests.UseCasesTests
{
    public class BaseUseCaseTest : IClassFixture<MapperFixture>
    {
        protected IMapper Converter { get; }

        protected BaseUseCaseTest()
        {
            var mapper = new MapperFixture();
            Converter = mapper.CreateMapper();
        }
    }
}