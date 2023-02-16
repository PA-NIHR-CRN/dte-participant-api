using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Enrichers;
using Domain.Entities.ParticipantRegistrations;
using Dte.Common.Contracts;
using Dte.Common.Exceptions;
using MediatR;

namespace Application.Participants.V1.Commands.ParticipantRegistrations
{
    public class CreateParticipantRegistrationCommand : IRequest
    {
        public long StudyId { get; }
        public string SiteId { get; }
        public string ParticipantId { get; }

        public CreateParticipantRegistrationCommand(long studyId, string siteId, string participantId)
        {
            StudyId = studyId;
            SiteId = siteId;
            ParticipantId = participantId;
        }
        
        public class CreateParticipantRegistrationCommandHandler : IRequestHandler<CreateParticipantRegistrationCommand>
        {
            private readonly IParticipantRegistrationRepository _participantRegistrationRepository;
            private readonly IEnricher<ParticipantRegistration> _participantRegistrationEnricher;
            private readonly IClock _clock;

            public CreateParticipantRegistrationCommandHandler(IParticipantRegistrationRepository participantRegistrationRepository,
                IEnricher<ParticipantRegistration> participantRegistrationEnricher,
                IClock clock)
            {
                _participantRegistrationRepository = participantRegistrationRepository;
                _participantRegistrationEnricher = participantRegistrationEnricher;
                _clock = clock;
            }
            
            public async Task<Unit> Handle(CreateParticipantRegistrationCommand request, CancellationToken cancellationToken)
            {
                // TODO - create a Participant study validator or Rules engine
                var participantRegistrationsForStudy = await _participantRegistrationRepository.GetParticipantsByStudyAsync(request.StudyId, request.ParticipantId);
                    
                // Check if participant has these states on any other site in this study
                var participantAcceptedSites = participantRegistrationsForStudy.Where
                (
                    x => x.ParticipantRegistrationStatus == ParticipantRegistrationStatus.Applied ||
                         x.ParticipantRegistrationStatus == ParticipantRegistrationStatus.Enrolled ||
                         x.ParticipantRegistrationStatus == ParticipantRegistrationStatus.Screening
                ).ToList();
                
                if (participantAcceptedSites.Any())
                {
                    var errorList = participantAcceptedSites
                        .Select(participantAcceptedSite => $"Participant registration already exists for studyId: {request.StudyId} - siteId's: {participantAcceptedSite.SiteId} - Status: {participantAcceptedSite.ParticipantRegistrationStatus}")
                        .ToList();

                    throw new ConflictException(string.Join("; ", errorList));
                }
                
                var entity = new ParticipantRegistration
                {
                    StudyId = request.StudyId,
                    SiteId = request.SiteId,
                    ParticipantId = request.ParticipantId,
                    SubmittedAtUtc = _clock.Now(),
                    ParticipantRegistrationStatus = ParticipantRegistrationStatus.Applied
                };

                await _participantRegistrationRepository.CreateParticipantRegistrationAsync(await _participantRegistrationEnricher.Enrich(entity));

                return Unit.Value;
            }
        }
    }
}