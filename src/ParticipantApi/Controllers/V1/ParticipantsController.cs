using System.Threading.Tasks;
using Application.Participants.V1.Commands.ParticipantRegistrations;
using Application.Participants.V1.Commands.Participants;
using Application.Participants.V1.Queries.Participants;
using Application.Responses.V1.Participants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParticipantApi.Mappers.Participants;
using ParticipantApi.Requests.Participants;
using Swashbuckle.AspNetCore.Annotations;

namespace ParticipantApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class ParticipantsController : Controller
    {
        private readonly IMediator _mediator;

        public ParticipantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get participant details
        /// </summary>
        /// <response code="200">Participant details retrieved</response>
        /// <response code="404">Participant details not found</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ParticipantDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{participantId}/details")]
        public async Task<IActionResult> GetParticipantDetails(string participantId)
        {
            return Ok(await _mediator.Send(new GetParticipantDetailsQuery(participantId)));
        }
        
        /// <summary>
        /// Get participant details by email
        /// </summary>
        /// <response code="200">Participant details retrieved</response>
        /// <response code="404">Participant details not found</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ParticipantDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{participantId}/detailsbyemail")]
        public async Task<IActionResult> GetParticipantDetailsByEmail(string email)
        {
            return Ok(await _mediator.Send(new GetParticipantDetailsByEmailQuery(email)));
        }

        /// <summary>
        /// Create participant details
        /// </summary>
        /// <response code="200">Participant details created</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPost("details")]
        public async Task<IActionResult> CreateParticipantDetails([FromBody] CreateParticipantDetailsRequest request)
        {
            var command = new CreateParticipantDetailsCommand
            (
                request.ParticipantId,
                request.Email,
                request.Firstname,
                request.Lastname,
                request.ConsentRegistration,
                request.NhsId
            );

            await _mediator.Send(command);
                
            return Ok();
        }
        
        /// <summary>
        /// Update participant details
        /// </summary>
        /// <response code="200">Participant details updated</response>
        /// <response code="404">Participant details not found</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPut("{participantId}/details")]
        public async Task<IActionResult> UpdateParticipantDetails(string participantId, [FromBody] UpdateParticipantDetailsRequest request)
        {
            if (string.IsNullOrWhiteSpace(participantId))
            {
                return BadRequest($"{nameof(participantId)} is required");
            }
            
            var command = new UpdateParticipantDetailsCommand
            (
                participantId,
                request.Firstname,
                request.Lastname,
                request.ConsentRegistration
            );

            await _mediator.Send(command);
                
            return Ok();
        }
        
        /// <summary>
        /// Get participant demographics
        /// </summary>
        /// <response code="200">Participant demographics retrieved</response>
        /// <response code="404">Participant demographics not found</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ParticipantDemographicsResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{participantId}/demographics")]
        public async Task<IActionResult> GetParticipantDemographics(string participantId)
        {
            return Ok(await _mediator.Send(new GetParticipantDemographicsQuery(participantId)));
        }
        
        /// <summary>
        /// Create participant demographics
        /// </summary>
        /// <response code="200">Participant demographics created</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPost("demographics")]
        public async Task<IActionResult> CreateParticipantDemographics([FromBody] CreateParticipantDemographicsRequest request)
        {
            var command = new CreateParticipantDemographicsCommand
            (
                request.ParticipantId,
                request.MobileNumber,
                request.LandlineNumber,
                ParticipantRequestMapper.MapTo(request.Address),
                request.DateOfBirth,
                request.SexRegisteredAtBirth,
                request.GenderIsSameAsSexRegisteredAtBirth,
                request.EthnicGroup,
                request.EthnicBackground,
                request.Disability,
                request.DisabilityDescription,
                request.HealthConditionInterests
            );

            await _mediator.Send(command);
                
            return Ok();
        }
        
        /// <summary>
        /// Update participant demographics
        /// </summary>
        /// <response code="200">Participant demographics updated</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPut("{participantId}/demographics")]
        public async Task<IActionResult> UpdateParticipantDemographics(string participantId, [FromBody] UpdateParticipantDemographicsRequest request)
        {
            if (string.IsNullOrWhiteSpace(participantId))
            {
                return BadRequest($"{nameof(participantId)} is required");
            }
            
            var command = new UpdateParticipantDemographicsCommand
            (
                participantId,
                request.MobileNumber,
                request.LandlineNumber,
                ParticipantRequestMapper.MapTo(request.Address),
                request.DateOfBirth,
                request.SexRegisteredAtBirth,
                request.GenderIsSameAsSexRegisteredAtBirth,
                request.EthnicGroup,
                request.EthnicBackground,
                request.Disability,
                request.DisabilityDescription,
                request.HealthConditionInterests
            );

            await _mediator.Send(command);
            
            return Ok();
        }
        
        /// <summary>
        /// Check a participant is suitable for this study
        /// </summary>
        /// <response code="200">Participant suitability information retrieved</response>
        /// /// <response code="404">Participant suitability not found</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ParticipantSuitabilityResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("suitability/{studyId:long}/participant/{participantId}")]
        public async Task<IActionResult> GetParticipantSuitability(long studyId, string participantId)
        {
            return Ok(await _mediator.Send(new GetParticipantSuitabilityQuery(studyId, participantId)));
        }
        
        /// <summary>
        /// Create a participant registration for a Study Site
        /// </summary>
        /// <response code="200">Participant registration created</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPost("participantregistrations")]
        public async Task<IActionResult> CreateParticipantRegistration([FromBody] CreateParticipantRegistrationRequest request)
        {
            var participantSuitabilityResponse = await _mediator.Send(new GetParticipantSuitabilityQuery(request.StudyId, request.ParticipantId));

            if (!participantSuitabilityResponse.IsSuitable)
            {
                return UnprocessableEntity(new { Message = "Participant is not suitable for this study" });
            }

            await _mediator.Send(new CreateParticipantRegistrationCommand
            (
                request.StudyId,
                request.SiteId,
                request.ParticipantId
            ));
                
            return Ok();
        }
        
        /// <summary>
        /// Delete a participant account
        /// </summary>
        /// <response code="200">Participant deleted</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpDelete("deleteparticipantaccount")]
        public async Task<IActionResult> DeleteParticipantAccount([FromBody] DeleteParticipantAccountRequest request)
        {
            await _mediator.Send(new RemoveParticipantConsentCommand(request.ParticipantId));
                
            return Ok();
        }
        
        /// <summary>
        /// Update participant email address
        /// </summary>
        /// <response code="200">Participant email updated</response>
        /// <response code="404">Participant details not found</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPut("{participantId}/updateparticipantemail")]
        public async Task<IActionResult> UpdateParticipantEmail(string participantId, [FromBody] UpdateParticipantEmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(participantId))
            {
                return BadRequest($"{nameof(participantId)} is required");
            }
            
            var command = new UpdateParticipantEmailCommand(participantId, request.Email);

            await _mediator.Send(command);
                
            return Ok();
        }
    }
}