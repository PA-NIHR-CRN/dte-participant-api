using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Dte.Common.Exceptions;
using Dte.Common.Contracts;
using MediatR;

namespace Application.Participants.V1.Commands.Participants
{
    public class UpdateParticipantDetailsCommand : IRequest
    {
        public string ParticipantId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool ConsentRegistration { get; }

        public UpdateParticipantDetailsCommand(string participantId, string firstname, string lastname, bool consentRegistration)
        {
            ParticipantId = participantId;
            Firstname = firstname;
            Lastname = lastname;
            ConsentRegistration = consentRegistration;
        }
        
        public class UpdateParticipantDetailsCommandHandler : IRequestHandler<UpdateParticipantDetailsCommand>
        {
            private readonly IParticipantRepository _participantRepository;
            private readonly IClock _clock;

            public UpdateParticipantDetailsCommandHandler(IParticipantRepository participantRepository, IClock clock)
            {
                _participantRepository = participantRepository;
                _clock = clock;
            }
            
            public async Task<Unit> Handle(UpdateParticipantDetailsCommand request, CancellationToken cancellationToken)
            {
                var entity = await _participantRepository.GetParticipantDetailsAsync(request.ParticipantId);
                
                if (entity == null)
                {
                    throw new NotFoundException($"Participant not found, Id: {request.ParticipantId}");
                }

                entity.Firstname = request.Firstname;
                entity.Lastname = request.Lastname;
                entity.ConsentRegistration = request.ConsentRegistration;
                entity.ConsentRegistrationAtUtc = request.ConsentRegistration ? _clock.Now() : (DateTime?)null;
                entity.UpdatedAtUtc = _clock.Now();

                await _participantRepository.UpdateParticipantDetailsAsync(entity);
                
                return Unit.Value;
            }
        }
    }
}