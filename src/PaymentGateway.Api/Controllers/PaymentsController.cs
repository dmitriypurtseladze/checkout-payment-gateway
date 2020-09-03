using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Models;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{apiVersion:apiVersion}/payments")]
    public class PaymentsController : BaseController
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Processes a payment.
        /// ApiVersion: 1.0
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Payment</returns>
        [HttpPost]
        [Authorize(Policy = Policies.ApiKey)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<PaymentResponse> ProcessPaymentAsync([FromBody] ProcessPaymentRequest request)
        {
            var command = new ProcessPaymentCommand(request);
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Returns a payment by id.
        /// ApiVersion: 1.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Payment</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.ApiKey)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<PaymentResponse> GetOnePaymentAsync(Guid id)
        {
            var query = new GetOnePaymentQuery(id);
            return await _mediator.Send(query);
        }
    }
}