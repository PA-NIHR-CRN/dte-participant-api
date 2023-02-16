using Application.Responses.V1.Participants;
using Domain.Entities.Participants;

namespace Application.Mappings.Participants
{
    public static class ParticipantMapper
    {
        public static ParticipantDetailsResponse MapTo(ParticipantDetails source)
        {
            return new ParticipantDetailsResponse
            {
                ParticipantId = source.ParticipantId,
                Email = source.Email,
                Firstname = source.Firstname,
                Lastname = source.Lastname,
                ConsentRegistration = source.ConsentRegistration,
                ConsentRegistrationAtUtc = source.ConsentRegistrationAtUtc,
                RemovalOfConsentRegistrationAtUtc = source.RemovalOfConsentRegistrationAtUtc,
                HasDemographics = source.HasDemographics,
                NhsId = source.NhsId,
            };
        }
        
        public static ParticipantDemographicsResponse MapTo(ParticipantDemographics source)
        {
            return new ParticipantDemographicsResponse
            {
                MobileNumber = source.MobileNumber,
                LandlineNumber = source.LandlineNumber,
                Address = ParticipantAddressMapper.MapTo(source.Address),
                DateOfBirth = source.DateOfBirth,
                SexRegisteredAtBirth = source.SexRegisteredAtBirth,
                GenderIsSameAsSexRegisteredAtBirth = source.GenderIsSameAsSexRegisteredAtBirth,
                EthnicGroup = source.EthnicGroup,
                EthnicBackground = source.EthnicBackground,
                Disability = source.Disability,
                DisabilityDescription = source.DisabilityDescription,
                HealthConditionInterests = source.HealthConditionInterests,
                HasDemographics = source.HasDemographics
            };
        }
    }
}