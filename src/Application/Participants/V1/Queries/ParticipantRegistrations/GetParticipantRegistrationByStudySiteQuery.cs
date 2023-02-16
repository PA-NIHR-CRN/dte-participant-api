using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Mappings.Participants;
using Application.Responses.V1.Participants;
using Dte.Common.Exceptions;
using MediatR;

namespace Application.Participants.V1.Queries.ParticipantRegistrations
{
    public class GetParticipantRegistrationByStudySiteQuery : IRequest<ParticipantRegistrationResponse>
    {
        public long StudyId { get; }
        public string SiteId { get; }
        public string ParticipantId { get; }

        public GetParticipantRegistrationByStudySiteQuery(long studyId, string siteId, string participantId)
        {
            StudyId = studyId;
            SiteId = siteId;
            ParticipantId = participantId;
        }
        
        public class GetParticipantByStudySiteQueryHandler : IRequestHandler<GetParticipantRegistrationByStudySiteQuery, ParticipantRegistrationResponse>
        {
            private readonly IParticipantRegistrationRepository _participantRegistrationRepository;

            public GetParticipantByStudySiteQueryHandler(IParticipantRegistrationRepository participantRegistrationRepository)
            {
                _participantRegistrationRepository = participantRegistrationRepository;
            }

            public async Task<ParticipantRegistrationResponse> Handle(GetParticipantRegistrationByStudySiteQuery request, CancellationToken cancellationToken)
            {
                var participantRegistration = (await _participantRegistrationRepository.GetParticipantByStudySiteAsync(request.StudyId, request.SiteId, request.ParticipantId));
                
                if (participantRegistration == null)
                {
                    throw new NotFoundException($"No participant registration found for studyId: {request.StudyId} and siteId: {request.SiteId} and participantId: {request.ParticipantId}");
                }

                return ParticipantRegistrationMapper.MapToWithMaskedPrivateDetails(participantRegistration);
            }
        }
    }
}