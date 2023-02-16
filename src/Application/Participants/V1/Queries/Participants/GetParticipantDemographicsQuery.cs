using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Mappings.Participants;
using Application.Responses.V1.Participants;
using Dte.Common.Exceptions;
using MediatR;

namespace Application.Participants.V1.Queries.Participants
{
    public class GetParticipantDemographicsQuery : IRequest<ParticipantDemographicsResponse>
    {
        public string ParticipantId { get; set; }

        public GetParticipantDemographicsQuery(string participantId)
        {
            ParticipantId = participantId;
        }
        
        public class GetParticipantDemographicsQueryHandler : IRequestHandler<GetParticipantDemographicsQuery, ParticipantDemographicsResponse>
        {
            private readonly IParticipantRepository _participantRepository;

            public GetParticipantDemographicsQueryHandler(IParticipantRepository participantRepository)
            {
                _participantRepository = participantRepository;
            }
            
            public async Task<ParticipantDemographicsResponse> Handle(GetParticipantDemographicsQuery request, CancellationToken cancellationToken)
            {
                var participantDemographics = await _participantRepository.GetParticipantDemographicsAsync(request.ParticipantId);

                if (participantDemographics == null)
                {
                    throw new NotFoundException($"No participant demographics found for participantId: {request.ParticipantId}");
                }

                return ParticipantMapper.MapTo(participantDemographics);
            }
        }
    }
}