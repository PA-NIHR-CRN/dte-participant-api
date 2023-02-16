using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Mappings.Participants;
using Application.Responses.V1.Participants;
using Dte.Common.Exceptions;
using MediatR;

namespace Application.Participants.V1.Queries.Participants
{
    public class GetParticipantDetailsQuery : IRequest<ParticipantDetailsResponse>
    {
        public string ParticipantId { get; set; }

        public GetParticipantDetailsQuery(string participantId)
        {
            ParticipantId = participantId;
        }
        
        public class GetParticipantDetailsQueryHandler : IRequestHandler<GetParticipantDetailsQuery, ParticipantDetailsResponse>
        {
            private readonly IParticipantRepository _participantRepository;

            public GetParticipantDetailsQueryHandler(IParticipantRepository participantRepository)
            {
                _participantRepository = participantRepository;
            }
            
            public async Task<ParticipantDetailsResponse> Handle(GetParticipantDetailsQuery request, CancellationToken cancellationToken)
            {
                var participantDetails = await _participantRepository.GetParticipantDetailsAsync(request.ParticipantId);

                if (participantDetails == null)
                {
                    throw new NotFoundException($"No participant details found for participantId: {request.ParticipantId}");
                }

                return ParticipantMapper.MapTo(participantDetails);
            }
        }
    }
}