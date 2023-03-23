using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Domain.Entities.Participants;
using Dte.Common.Contracts;
using MediatR;

namespace Application.Participants.V1.Commands.Participants
{
    public class RemoveParticipantConsentCommand : IRequest
    {
        private string ParticipantId { get; }

        public RemoveParticipantConsentCommand(string participantId)
        {
            ParticipantId = participantId;
        }

        public class Handler : IRequestHandler<RemoveParticipantConsentCommand>
        {
            private readonly IParticipantRepository _participantRepository;
            private readonly IClock _clock;

            public Handler(IParticipantRepository participantRepository, IClock clock)
            {
                _participantRepository = participantRepository;
                _clock = clock;
            }

            private async Task RemoveDetails(ParticipantDetails entity)
            {
                entity.Firstname = entity.Lastname = entity.Email = null;
                entity.ConsentRegistration = false;
                entity.RemovalOfConsentRegistrationAtUtc = entity.UpdatedAtUtc = _clock.Now();

                await _participantRepository.UpdateParticipantDetailsAsync(entity);
            }

            private async Task RemoveDemographics(ParticipantDemographics demographics)
            {
                demographics.MobileNumber = demographics.LandlineNumber = null;
                demographics.Address?.Clear();

                await _participantRepository.UpdateParticipantDemographicsAsync(demographics);
            }

            private async Task RemoveParticipantData(ParticipantDetails entity)
            {
                await RemoveDetails(entity);
                
                var demographics = await _participantRepository.GetParticipantDemographicsAsync(entity.Pk.Replace("PARTICIPANT#", ""));
                await RemoveDemographics(demographics);
            }

            public async Task<Unit> Handle(RemoveParticipantConsentCommand request, CancellationToken cancellationToken)
            {
                var entity = await _participantRepository.GetParticipantDetailsAsync(request.ParticipantId);
                if (entity == null) return Unit.Value;

                var linkedEmail = entity.Email;

                await RemoveParticipantData(entity);

                var linkedEntity = await _participantRepository.GetParticipantDetailsByEmailAsync(linkedEmail);
                if (linkedEntity == null) return Unit.Value;
                await RemoveParticipantData(linkedEntity);

                return Unit.Value;
            }
        }
    }
}
