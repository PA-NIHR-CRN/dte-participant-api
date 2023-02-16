using System.Threading.Tasks;
using Application.Participants.V1.Queries.ParticipantRegistrations;
using Domain.Entities.ParticipantRegistrations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ParticipantApi.Controllers.V2
{
    [ApiController]
    [ApiVersion("2")]
    [Route("api/studies")]
    public class ParticipantRegistrationsController : Controller
    {
        private readonly IMediator _mediator;

        public ParticipantRegistrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{studyId:long}/participants/status/{status}")]
        public async Task<IActionResult> GetParticipantRegistrationsStatusByStudy(long studyId, ParticipantRegistrationStatus status)
        {
            var participantRegistrationModels = await _mediator.Send(new GetParticipantRegistrationsStatusByStudyQuery(studyId, status));
            
            return Ok(new {Version = "2", participantRegistrationModels});
        }
    }
}