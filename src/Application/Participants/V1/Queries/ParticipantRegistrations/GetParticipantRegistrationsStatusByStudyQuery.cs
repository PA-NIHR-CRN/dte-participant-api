using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Mappings.Participants;
using Application.Responses.V1.Participants;
using Dte.Common.Exceptions;
using Domain.Entities.ParticipantRegistrations;
using MediatR;

namespace Application.Participants.V1.Queries.ParticipantRegistrations
{
    public class GetParticipantRegistrationsStatusByStudyQuery : IRequest<IEnumerable<ParticipantRegistrationResponse>>
    {
        public long StudyId { get; set; }
        public ParticipantRegistrationStatus Status { get; }

        public GetParticipantRegistrationsStatusByStudyQuery(long studyId, ParticipantRegistrationStatus status)
        {
            StudyId = studyId;
            Status = status;
        }
        
        public class GetParticipantRegistrationsStatusByStudyQueryHandler : IRequestHandler<GetParticipantRegistrationsStatusByStudyQuery, IEnumerable<ParticipantRegistrationResponse>>
        {
            private readonly IParticipantRegistrationRepository _participantRegistrationRepository;

            public GetParticipantRegistrationsStatusByStudyQueryHandler(IParticipantRegistrationRepository participantRegistrationRepository)
            {
                _participantRegistrationRepository = participantRegistrationRepository;
            }

            public async Task<IEnumerable<ParticipantRegistrationResponse>> Handle(GetParticipantRegistrationsStatusByStudyQuery request, CancellationToken cancellationToken)
            {
                var participantRegistrations = (await _participantRegistrationRepository.GetParticipantRegistrationsStatusByStudy(request.StudyId, request.Status)).ToList();

                if (!participantRegistrations.Any())
                {
                    throw new NotFoundException($"No participant registrations found with status: {request.Status}");
                }

                return participantRegistrations.Select(ParticipantRegistrationMapper.MapToWithMaskedPrivateDetails);
            }
        }
    }
}