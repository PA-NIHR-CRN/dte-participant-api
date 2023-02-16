using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Dte.Common.Exceptions;
using Domain.Entities.ParticipantRegistrations;
using Dte.Common.Contracts;
using MediatR;

namespace Application.Participants.V1.Commands.ParticipantRegistrations
{
    public class SetScreeningParticipantRegistrationCommand : IRequest
    {
        public long StudyId { get; set; }
        public string SiteId { get; set; }
        public string ParticipantId { get; set; }

        public SetScreeningParticipantRegistrationCommand(long studyId, string siteId, string participantId)
        {
            StudyId = studyId;
            SiteId = siteId;
            ParticipantId = participantId;
        }
        
        public class SetScreeningParticipantRegistrationCommandHandler : IRequestHandler<SetScreeningParticipantRegistrationCommand>
        {
            private readonly IParticipantRegistrationRepository _participantRegistrationRepository;
            private readonly IClock _clock;

            public SetScreeningParticipantRegistrationCommandHandler(IParticipantRegistrationRepository participantRegistrationRepository, IClock clock)
            {
                _participantRegistrationRepository = participantRegistrationRepository;
                _clock = clock;
            }

            public async Task<Unit> Handle(SetScreeningParticipantRegistrationCommand request, CancellationToken cancellationToken)
            {
                var participantRegistration = await _participantRegistrationRepository.GetParticipantByStudySiteAsync(request.StudyId, request.SiteId, request.ParticipantId);
                
                if (participantRegistration == null)
                {
                    throw new NotFoundException($"No participant registration found for studyId: {request.StudyId} and siteId: {request.SiteId} and participantId: {request.ParticipantId}");
                }

                if (participantRegistration.ParticipantRegistrationStatus == ParticipantRegistrationStatus.Screening)
                {
                    return Unit.Value;
                }
                
                participantRegistration.ParticipantRegistrationStatus = ParticipantRegistrationStatus.Screening;
                participantRegistration.UpdatedAtUtc = _clock.Now();

                await _participantRegistrationRepository.SaveParticipantRegistrationAsync(participantRegistration);

                return Unit.Value;
            }
        }
    }
}