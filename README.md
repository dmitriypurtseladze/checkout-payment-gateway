#Payment Gateway: 
Responsible for validating requests, storing card information, forwarding payment requests, and accepting payment responses to and from the acquiring bank.

#Setup instructions:
 Please run the following commands:
 - docker-compose build
 - docker-compose up -d
 
 The build runs Unit tests and Integration tests.
 
 #API description and documentation:
  To check and try the endpoints please go to http://localhost:8090/swagger/index.html.
  There is swagger documentation with an example of how to use API endpoints.
  
  The main list of used API endpoints:
  
  | Name           | Method      | URL                      | Protected |
  | ---            | ---         | ---                      | ---       |
  | ProcessPayment | `POST`      | `/api/v1.0/payments`     | ✓         |
  | GetOnePayment  | `GET`       | `/api/v1.0/payments/{id}`| ✓         |
  | Ping           | `GET`       | `/ping`                  | ✘         |
  | Health         | `GET`       | `/health`                | ✘         |
  | Metrics        | `GET`       | `/metrics`               | ✘         |
  
  ProcessPayment API request example:
  - ```curl --location --request POST 'http://localhost:8090/api/v1.0/payments' --header 'x-api-key: checkout' --header 'Content-Type: application/json' --data-raw '{"cvv":123,"cardNumber":"4444-4444-4444-4444","expiry":"12/21","fullName":"Dmitriy Purtseladze","amount":100,"currency":"EUR"}'```
  
  #Design decisions:
  The application was built according to SOLID principles, Domain-Driven Design, Onion Architecture, and Command and Query Responsibility Segregation (CQRS) patterns. 
  The main purpose of that is to have clean code and architecture. Moreover, it's easier to build microservices using those approaches.
  
  The solution is using the following layers:
  
  - API - contains API layer;
  - Application - provides layer with the main application logic, commands, queries, request handlers ("UseCases"), validators, mappings settings and etc;
  - Infrastructure - provides database connections, repositories, and/or all the connections to the external services, such as message busses, cache services or bank service;
  - Domain - contains the main database entities;
  - Models - the classes that represents the data of API;
  - Backend.Common - contains the code that can be used by other microservices. Can be implemented as nuget package or git submodule;
  - UnitTests - the projects contain unit tests that check the individual units of the application;
  - IntegrationTests - the project contains tests that are connected to the in-memory database.
  
  API is protected by the key "checkout". Even though it is not the most secure authentication method for the web application, it's the easiest and fastest way to provide minimum API security and demonstrate how AuthorizationHandler and Policies can be used in the application. 
  The best way to implement security would be to use IdentityServer4 or Google Identity Platform that provides OpenID Connect specification for authentication.
  
  Bank service simulator that randomly returns a successful or unsuccessful response. 
  This component can be easily switched to a real bank, because ```IBankService``` interface is used in the code.
  In addition the component can be extended with Polly library (https://github.com/App-vNext/Polly) by applying Retry, Circuit breaker and Timeout policies.
  
  Postgres database is used in the application.
  There is a migration script that runs on the API startup and creates a database schema with a table ```Payments```.
  The table structure is the following:
  
      "Id"                uuid      not null constraint "PK_Payments" primary key,
      "CreatedAt"         timestamp not null,
      "CardNumber"        text      not null,
      "Expiry"            text      not null,
      "FullName"          text      not null,
      "Amount"            real      not null,
      "Currency"          text      not null,
      "BankPaymentId"     text      not null,
      "BankPaymentStatus" text      not null
  
  On every ```POST``` request ```CardNumber``` is encrypted by the Advanced Encryption Standard (AES) and stored in the database. 
  Encryption is made using the secret key, that is provided as environment variable ```Aes_Key``` and must be stored in a key vault. 
  On ```GET``` request the code decrypts the number, masks and returns it to the API client.
  
  #Libraries used:
  - Swagger - provides UI with up to date documentation;
  - MediatR - helps to implement CQRS pattern;
  - Autofac - used for dependencies injection and helps to reduce coupling between the application layers, because every layer can be "packed" as a module;
  - xunit - provides an easy unit testing;
  - NSubstitute - helps to mock dependencies for the unit tests;
  - Serilog - makes it easier to setup and maintain different types of logging, such as a console, a file, etc.
  - Npgsql.EntityFrameworkCore - used to interact with Postgres database using EF core;
  - Microsoft.EntityFrameworkCore.Design - helps to setup database migration scripts.
  - App.Metrics.AspNetCore - provides functionality for application monitoring