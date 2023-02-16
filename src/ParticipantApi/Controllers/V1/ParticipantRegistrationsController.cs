using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Participants.V1.Commands.ParticipantRegistrations;
using Application.Participants.V1.Queries.ParticipantRegistrations;
using Application.Responses.V1.Participants;
using Domain.Entities.ParticipantRegistrations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ParticipantApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/studies")]
    public class ParticipantRegistrationsController : Controller
    {
        private readonly IMediator _mediator;

        public ParticipantRegistrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get participant registrations for a study site
        /// </summary>
        /// <response code="200">Participant registrations retrieved</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ParticipantRegistrationResponse>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{studyId:long}/sites/{siteId}/participants")]
        public async Task<IActionResult> GetParticipantRegistrationsByStudySite(long studyId, string siteId)
        {
            return Ok(await _mediator.Send(new GetParticipantRegistrationsByStudySiteQuery(studyId, siteId)));
        }
        
        /// <summary>
        /// Get participant registrations for a study
        /// </summary>
        /// <response code="200">Participant registrations retrieved</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ParticipantRegistrationResponse>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{studyId:long}/participants/{participantId}")]
        public async Task<IActionResult> GetParticipantsRegistrationsByStudy(long studyId, string participantId)
        {
            return Ok(await _mediator.Send(new GetParticipantsRegistrationByStudyQuery(studyId, participantId)));
        }
        
        /// <summary>
        /// Get s participant registrations for a study site
        /// </summary>
        /// <response code="200">Participant registration retrieved</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ParticipantRegistrationResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{studyId:long}/sites/{siteId}/participants/{participantId}")]
        public async Task<IActionResult> GetParticipantRegistrationByStudySite(long studyId, string siteId, string participantId)
        {
            return Ok(await _mediator.Send(new GetParticipantRegistrationByStudySiteQuery(studyId, siteId, participantId)));
        }
        
        /// <summary>
        /// Get participant registrations for a study site by status
        /// </summary>
        /// <response code="200">Participant registrations retrieved</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ParticipantRegistrationResponse>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{studyId:long}/sites/{siteId}/participantstatus/{status}")]
        public async Task<IActionResult> GetParticipantsRegistrationByStudySiteStatus(long studyId, string siteId, ParticipantRegistrationStatus status)
        {
            return Ok(await _mediator.Send(new GetParticipantRegistrationsByStudySiteStatusQuery(studyId, siteId, status)));
        }
        
        /// <summary>
        /// Get participant registrations for a study and status
        /// </summary>
        /// <response code="200">Participant registrations retrieved</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ParticipantRegistrationResponse>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpGet("{studyId:long}/participants/status/{status}")]
        public async Task<IActionResult> GetParticipantRegistrationsStatusByStudy(long studyId, ParticipantRegistrationStatus status)
        {
            return Ok(await _mediator.Send(new GetParticipantRegistrationsStatusByStudyQuery(studyId, status)));
        }

        /// <summary>
        /// Set a participant to status of screening
        /// </summary>
        /// <response code="200">Participant registration set to screening</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPost("{studyId:long}/sites/{siteId}/participants/{participantId}/screening")]
        public async Task<IActionResult> SetScreeningParticipantRegistration(long studyId, string siteId, string participantId)
        {
            var command = new SetScreeningParticipantRegistrationCommand(studyId, siteId, participantId);
            
            await _mediator.Send(command);
                
            return Ok();
        }
        
        /// <summary>
        /// Set a participant to status of not selected
        /// </summary>
        /// <response code="200">Participant registration set to not selected</response>
        /// <response code="500">Server side error</response>
        [SwaggerResponse(StatusCodes.Status200OK, Type = null)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = null)]
        [HttpPost("{studyId:long}/sites/{siteId}/participants/{participantId}/notselected")]
        public async Task<IActionResult> SetNotSelectedParticipantRegistration(long studyId, string siteId, string participantId)
        {
            var command = new SetNotSelectedParticipantRegistrationCommand(studyId, siteId, participantId);
            
            await _mediator.Send(command);
                
            return Ok();
        }
    }
}