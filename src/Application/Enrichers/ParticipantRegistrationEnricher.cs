using System.Diagnostics;
using System.Threading.Tasks;
using Application.Contracts;
using Domain.Entities.ParticipantRegistrations;
using Microsoft.Extensions.Logging;

namespace Application.Enrichers
{
    public class ParticipantRegistrationEnricher : IEnricher<ParticipantRegistration>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ILogger<ParticipantRegistrationEnricher> _logger;

        public ParticipantRegistrationEnricher(IParticipantRepository participantRepository, ILogger<ParticipantRegistrationEnricher> logger)
        {
            _participantRepository = participantRepository;
            _logger = logger;
        }
        
        public async Task<ParticipantRegistration> Enrich(ParticipantRegistration source)
        {
            await EnrichDetails(source);
            await EnrichDemographics(source);

            return source;
        }

        private async Task EnrichDetails(ParticipantRegistration source)
        {
            var sw = Stopwatch.StartNew();
            var participantDetails = await _participantRepository.GetParticipantDetailsAsync(source.ParticipantId);
            _logger.LogInformation($"*** _participantRepository.GetParticipantDetailsAsync: {sw.Elapsed}");

            if (participantDetails != null)
            {
                source.Email = participantDetails.Email;
                source.Firstname = participantDetails.Firstname;
                source.Lastname = participantDetails.Lastname;
                source.ConsentRegistration = participantDetails.ConsentRegistration;
                source.ConsentRegistrationAtUtc = participantDetails.ConsentRegistrationAtUtc;
                source.RemovalOfConsentRegistrationAtUtc = participantDetails.RemovalOfConsentRegistrationAtUtc;
            }
        }
        
        private async Task EnrichDemographics(ParticipantRegistration source)
        {
            var sw = Stopwatch.StartNew();
            var participantDemographics = await _participantRepository.GetParticipantDemographicsAsync(source.ParticipantId);
            _logger.LogInformation($"*** _participantRepository.GetParticipantDemographicsAsync: {sw.Elapsed}");

            if (participantDemographics != null)
            {
                source.MobileNumber = participantDemographics.MobileNumber;
                source.Lastname = participantDemographics.LandlineNumber;
                source.Address = participantDemographics.Address;
                source.DateOfBirth = participantDemographics.DateOfBirth;
                source.SexRegisteredAtBirth = participantDemographics.SexRegisteredAtBirth;
                source.GenderIsSameAsSexRegisteredAtBirth = participantDemographics.GenderIsSameAsSexRegisteredAtBirth;
                source.EthnicGroup = participantDemographics.EthnicGroup;
                source.EthnicBackground = participantDemographics.EthnicBackground;
                source.Disability = participantDemographics.Disability;
                source.DisabilityDescription = participantDemographics.DisabilityDescription;
                source.HealthConditionInterests = participantDemographics.HealthConditionInterests;
            }
        }
    }
}