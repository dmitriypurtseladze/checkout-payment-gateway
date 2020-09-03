FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY . .
WORKDIR /app/PaymentGateway.Api
RUN dotnet restore

RUN dotnet publish -c Release -o out

# Tests Start

FROM build

WORKDIR /app/PaymentGateway.Api.UnitTests
COPY ./PaymentGateway.Api.UnitTests ./
RUN dotnet test

WORKDIR /app/PaymentGateway.Application.UnitTests
COPY ./PaymentGateway.Application.UnitTests ./
RUN dotnet test

WORKDIR /app/IntegrationTests
COPY ./IntegrationTests ./
RUN dotnet test

# Tests End

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/PaymentGateway.Api/out ./

ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]