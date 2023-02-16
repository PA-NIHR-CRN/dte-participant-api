using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Responses.V1.Participants;
using Dte.Common.Exceptions;
using MediatR;

namespace Application.Participants.V1.Queries.Participants
{
    public class GetParticipantSuitabilityQuery : IRequest<ParticipantSuitabilityResponse>
    {
        public long StudyId { get; }
        public string ParticipantId { get; }

        public GetParticipantSuitabilityQuery(long studyId, string participantId)
        {
            StudyId = studyId;
            ParticipantId = participantId;
        }

        public class GetParticipantSuitabilityQueryHandler : IRequestHandler<GetParticipantSuitabilityQuery, ParticipantSuitabilityResponse>
        {
            private readonly IParticipantRepository _participantRepository;
            private readonly IStudyRepository _studyRepository;

            public GetParticipantSuitabilityQueryHandler(IParticipantRepository participantRepository, IStudyRepository studyRepository)
            {
                _participantRepository = participantRepository;
                _studyRepository = studyRepository;
            }

            public async Task<ParticipantSuitabilityResponse> Handle(GetParticipantSuitabilityQuery request, CancellationToken cancellationToken)
            {
                var study = await _studyRepository.GetStudyAsync(request.StudyId);
                if (study == null)
                {
                    throw new NotFoundException($"Study not found: {request.StudyId}");
                }
                
                var participantDemographics = await _participantRepository.GetParticipantDemographicsAsync(request.ParticipantId);
                if (participantDemographics == null)
                {
                    throw new NotFoundException($"Participant demographics not found for: {request.ParticipantId}");
                }

                // TODO - this will need to call a suitability engine - so let has hardcoded criteria for now. NIHRDIGENG-172
                var isSuitable = string.Equals("male", participantDemographics.SexRegisteredAtBirth, StringComparison.CurrentCultureIgnoreCase);
                
                return new ParticipantSuitabilityResponse { StudyTitle = study.Title, IsSuitable = isSuitable };
            }
        }
    }
}