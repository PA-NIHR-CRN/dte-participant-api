using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Mappings.Participants;
using Application.Responses.V1.Participants;
using Dte.Common.Exceptions;
using MediatR;

namespace Application.Participants.V1.Queries.ParticipantRegistrations
{
    public class GetParticipantsRegistrationByStudyQuery : IRequest<IEnumerable<ParticipantRegistrationResponse>>
    {
        public long StudyId { get; }
        public string ParticipantId { get; }

        public GetParticipantsRegistrationByStudyQuery(long studyId, string participantId)
        {
            StudyId = studyId;
            ParticipantId = participantId;
        }
        
        public class GetParticipantsByStudyQueryHandler : IRequestHandler<GetParticipantsRegistrationByStudyQuery, IEnumerable<ParticipantRegistrationResponse>>
        {
            private readonly IParticipantRegistrationRepository _participantRegistrationRepository;

            public GetParticipantsByStudyQueryHandler(IParticipantRegistrationRepository participantRegistrationRepository)
            {
                _participantRegistrationRepository = participantRegistrationRepository;
            }

            public async Task<IEnumerable<ParticipantRegistrationResponse>> Handle(GetParticipantsRegistrationByStudyQuery request, CancellationToken cancellationToken)
            {
                var participantRegistrations = (await _participantRegistrationRepository.GetParticipantsByStudyAsync(request.StudyId, request.ParticipantId)).ToList();
                
                if (!participantRegistrations.Any())
                {
                    throw new NotFoundException($"No participant registrations found for studyId: {request.StudyId} and participantId: {request.ParticipantId}");
                }

                return participantRegistrations.Select(ParticipantRegistrationMapper.MapToWithMaskedPrivateDetails);
            }
        }
    }
}