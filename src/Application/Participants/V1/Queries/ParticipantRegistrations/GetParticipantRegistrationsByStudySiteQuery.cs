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
    public class GetParticipantRegistrationsByStudySiteQuery : IRequest<IEnumerable<ParticipantRegistrationResponse>>
    {
        public long StudyId { get; }
        public string SiteId { get; }

        public GetParticipantRegistrationsByStudySiteQuery(long studyId, string siteId)
        {
            StudyId = studyId;
            SiteId = siteId; 
        }

        public class GetParticipantsByStudySiteQueryHandler : IRequestHandler<GetParticipantRegistrationsByStudySiteQuery, IEnumerable<ParticipantRegistrationResponse>>
        {
            private readonly IParticipantRegistrationRepository _participantRegistrationRepository;

            public GetParticipantsByStudySiteQueryHandler(IParticipantRegistrationRepository participantRegistrationRepository)
            {
                _participantRegistrationRepository = participantRegistrationRepository;
            }

            public async Task<IEnumerable<ParticipantRegistrationResponse>> Handle(GetParticipantRegistrationsByStudySiteQuery request, CancellationToken cancellationToken)
            {
                var participantRegistrations = (await _participantRegistrationRepository.GetParticipantsByStudySiteAsync(request.StudyId, request.SiteId)).ToList();
                
                if (!participantRegistrations.Any())
                {
                    throw new NotFoundException($"No participant registrations found for studyId: {request.StudyId} and siteId: {request.SiteId}");
                }

                return participantRegistrations.Select(ParticipantRegistrationMapper.MapToWithMaskedPrivateDetails);
            }
        }
    }
}