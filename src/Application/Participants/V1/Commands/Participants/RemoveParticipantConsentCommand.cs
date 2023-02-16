using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Dte.Common.Contracts;
using MediatR;

namespace Application.Participants.V1.Commands.Participants
{
    public class RemoveParticipantConsentCommand : IRequest
    {
        public string ParticipantId { get; set; }

        public RemoveParticipantConsentCommand(string participantId)
        {
            ParticipantId = participantId;
        }
        
        public class RemoveParticipantConsentCommandHandler : IRequestHandler<RemoveParticipantConsentCommand>
        {
            private readonly IParticipantRepository _participantRepository;
            private readonly IClock _clock;

            public RemoveParticipantConsentCommandHandler(IParticipantRepository participantRepository, IClock clock)
            {
                _participantRepository = participantRepository;
                _clock = clock;
            }
            
            public async Task<Unit> Handle(RemoveParticipantConsentCommand request, CancellationToken cancellationToken)
            {
                var entity = await _participantRepository.GetParticipantDetailsAsync(request.ParticipantId);
                
                if (entity == null)
                {
                    return Unit.Value;
                }

                entity.Firstname = null;
                entity.Lastname = null;
                entity.Email = null;
                entity.ConsentRegistration = false;
                entity.RemovalOfConsentRegistrationAtUtc = _clock.Now();
                entity.UpdatedAtUtc = _clock.Now();

                await _participantRepository.UpdateParticipantDetailsAsync(entity);


                var demographicsEntity = await _participantRepository.GetParticipantDemographicsAsync(request.ParticipantId);
                
                if (demographicsEntity == null)
                {
                    return Unit.Value;
                }

                demographicsEntity.MobileNumber = null;
                demographicsEntity.LandlineNumber = null;
                
                if (demographicsEntity.Address != null)
                {
                    demographicsEntity.Address.AddressLine1 = null;
                    demographicsEntity.Address.AddressLine2 = null;
                    demographicsEntity.Address.AddressLine3 = null;
                    demographicsEntity.Address.AddressLine4 = null;
                    demographicsEntity.Address.Town = null;
                }
                
                await _participantRepository.UpdateParticipantDemographicsAsync(demographicsEntity);
                
                return Unit.Value;
            }
        }
    }
}