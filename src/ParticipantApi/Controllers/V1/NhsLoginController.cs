using System.Threading.Tasks;
using Application.Enrichers;
using Application.Participants.V1.Commands.ParticipantRegistrations;
using Application.Participants.V1.Commands.Participants;
using Application.Participants.V1.Queries.Participants;
using Application.Responses.V1.Participants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParticipantApi.Mappers.Participants;
using ParticipantApi.Requests.Participants;
using Swashbuckle.AspNetCore.Annotations;

namespace ParticipantApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class NhsLoginController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NhsLoginController> _logger;

        public NhsLoginController(IMediator mediator, ILogger<NhsLoginController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Create participant details
        /// </summary>
        /// <response code="200">Participant details created</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPost("details")]
        public async Task<IActionResult> NhsCreateParticipantDetails([FromBody] CreateParticipantDetailsRequest request)
        {
            var command = new CreateParticipantDetailsNhsCommand
            (
                request.ParticipantId,
                request.Email,
                request.Firstname,
                request.Lastname,
                request.ConsentRegistration,
                request.NhsId,
                request.DateOfBirth,
                request.NhsNumber
            );

            _logger.LogDebug($"CreateParticipantDetailsRequest: {JsonConvert.SerializeObject(request)}");
            _logger.LogDebug($"CreateParticipantDetailsNhsCommandJsonConvert: {JsonConvert.SerializeObject(command)}");

            await _mediator.Send(command);

            return Ok();
        }
    }
}