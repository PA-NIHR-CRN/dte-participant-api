using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Dte.Common.Exceptions;
using Dte.Common.Contracts;
using MediatR;

namespace Application.Participants.V1.Commands.Participants
{
    public class UpdateParticipantEmailCommand : IRequest
    {
        public string ParticipantId { get; set; }
        public string Email { get; set; }

        public UpdateParticipantEmailCommand(string participantId, string email)
        {
            ParticipantId = participantId;
            Email = email;
        }
        
        public class UpdateParticipantEmailCommandHandler : IRequestHandler<UpdateParticipantEmailCommand>
        {
            private readonly IParticipantRepository _participantRepository;
            private readonly IClock _clock;

            public UpdateParticipantEmailCommandHandler(IParticipantRepository participantRepository, IClock clock)
            {
                _participantRepository = participantRepository;
                _clock = clock;
            }
            
            public async Task<Unit> Handle(UpdateParticipantEmailCommand request, CancellationToken cancellationToken)
            {
                var entity = await _participantRepository.GetParticipantDetailsAsync(request.ParticipantId);
                
                if (entity == null)
                {
                    throw new NotFoundException($"Participant not found, Id: {request.ParticipantId}");
                }

                entity.Email = request.Email.ToLower();
                entity.UpdatedAtUtc = _clock.Now();

                await _participantRepository.UpdateParticipantDetailsAsync(entity);
                
                return Unit.Value;
            }
        }
    }
}