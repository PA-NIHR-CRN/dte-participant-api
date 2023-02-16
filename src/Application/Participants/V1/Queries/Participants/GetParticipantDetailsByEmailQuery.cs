using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Mappings.Participants;
using Application.Responses.V1.Participants;
using Dte.Common.Exceptions;
using MediatR;

namespace Application.Participants.V1.Queries.Participants
{
    public class GetParticipantDetailsByEmailQuery : IRequest<ParticipantDetailsResponse>
    {
        public string Email { get; set; }

        public GetParticipantDetailsByEmailQuery(string email)
        {
            Email = email;
        }
        
        public class GetParticipantDetailsByEmailQueryHandler : IRequestHandler<GetParticipantDetailsByEmailQuery, ParticipantDetailsResponse>
        {
            private readonly IParticipantRepository _participantRepository;

            public GetParticipantDetailsByEmailQueryHandler(IParticipantRepository participantRepository)
            {
                _participantRepository = participantRepository;
            }
            
            public async Task<ParticipantDetailsResponse> Handle(GetParticipantDetailsByEmailQuery request, CancellationToken cancellationToken)
            {
                var participantDetails = await _participantRepository.GetParticipantDetailsByEmailAsync(request.Email);

                if (participantDetails == null)
                {
                    throw new NotFoundException($"No participant details found for email: {request.Email}");
                }

                return ParticipantMapper.MapTo(participantDetails);
            }
        }
    }
}