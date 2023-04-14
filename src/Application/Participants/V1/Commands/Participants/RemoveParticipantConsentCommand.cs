using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Application.Contracts;
using Application.Settings;
using Domain.Entities.Participants;
using Dte.Common.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

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
            private readonly IAmazonCognitoIdentityProvider _provider;
            private readonly AwsSettings _awsSettings;
            private readonly ILogger<RemoveParticipantConsentCommand> _logger;

            public Handler(IParticipantRepository participantRepository, IClock clock,
                IAmazonCognitoIdentityProvider provider, AwsSettings awsSettings,
                ILogger<RemoveParticipantConsentCommand> logger)
            {
                _participantRepository = participantRepository;
                _clock = clock;
                _provider = provider;
                _awsSettings = awsSettings;
                _logger = logger;
            }
            private static string DeletedKey(Guid primaryKey) => $"DELETED#{primaryKey}";
            private static string DeletedKey() => "DELETED#";
            private static string StripPrimaryKey(string pk) => pk.Replace("PARTICIPANT#", "");

            private async Task RemoveCognitoUser(string username)
            {
                await _provider.AdminDeleteUserAsync(new AdminDeleteUserRequest
                {
                    UserPoolId = _awsSettings.CognitoPoolId,
                    Username = username
                });
            }

            private async Task RemoveParticipantData(ParticipantDetails entity)
            {
                var participantId = StripPrimaryKey(entity.Pk);
                if (entity.NhsId == null)
                {
                    await RemoveCognitoUser(participantId);
                }

                await _participantRepository.DeleteParticipantDetailsAsync(entity);
            }
            
            private async Task SaveAnonymisedDemographicParticipantData(ParticipantDetails entity)
            {
                var primaryKey = DeletedKey(Guid.NewGuid());
                var anonEntity = new ParticipantDetails
                {
                    Pk = primaryKey,
                    Sk = DeletedKey(),
                    ConsentRegistration = false,
                    RemovalOfConsentRegistrationAtUtc = _clock.Now(),
                    UpdatedAtUtc = _clock.Now(),
                    CreatedAtUtc = entity.CreatedAtUtc,
                    NhsId = entity.NhsId ?? null,
                    ParticipantId = entity.ParticipantId ?? null,
                    DateOfBirth = entity.DateOfBirth
                };

                await _participantRepository.CreateAnonymisedDemographicParticipantDataAsync(anonEntity);
                
                var demographics = await _participantRepository.GetParticipantDemographicsAsync(StripPrimaryKey(entity.Pk));
                if (demographics == null) return;
                demographics.Pk = primaryKey;
                demographics.Sk = DeletedKey();
                demographics.MobileNumber = demographics.LandlineNumber = null;
                demographics.Disability = false;
                demographics.Address?.Clear();
                demographics.HealthConditionInterests?.Clear();

                await _participantRepository.UpdateParticipantDemographicsAsync(demographics);
                
            }

            public async Task<Unit> Handle(RemoveParticipantConsentCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = await _participantRepository.GetParticipantDetailsAsync(request.ParticipantId);
                    if (entity == null) return Unit.Value;

                    var linkedEmail = entity.Email;
                    await SaveAnonymisedDemographicParticipantData(entity);
                    await RemoveParticipantData(entity);
                    

                    var linkedEntity = await _participantRepository.GetParticipantDetailsByEmailAsync(linkedEmail);
                    if (linkedEntity == null) return Unit.Value;
                    await RemoveParticipantData(linkedEntity);
                }
                catch (Exception e)
                {
                    _logger.LogError("Delete-error = {EMessage}", e.Message);
                }

                return Unit.Value;
            }
        }
    }
}