using System;
using Application.Responses.V1.Participants;
using Domain.Entities.ParticipantRegistrations;

namespace Application.Mappings.Participants
{
    public static class ParticipantRegistrationMapper
    {
        public static ParticipantRegistrationResponse MapToWithMaskedPrivateDetails(ParticipantRegistration source)
        {
            return new ParticipantRegistrationResponse
            {
                StudyId = source.StudyId,
                SiteId = source.SiteId,
                ParticipantId = source.ParticipantId,
                ParticipantRegistrationStatus = source.ParticipantRegistrationStatus.ToString(),
                UpdatedAtUtc = source.UpdatedAtUtc,
                SubmittedAtUtc = source.SubmittedAtUtc,
                ParticipantDetails = new ParticipantDetailsResponse
                {
                    ParticipantId = source.ParticipantId,
                    Firstname = PrivateDetailsAllowed(source) ? source.Firstname : null,
                    Lastname = PrivateDetailsAllowed(source) ? source.Lastname : null,
                    Email = PrivateDetailsAllowed(source) ? source.Email : null,
                    ConsentRegistration = PrivateDetailsAllowed(source) && source.ConsentRegistration,
                    ConsentRegistrationAtUtc = PrivateDetailsAllowed(source) ? source.ConsentRegistrationAtUtc : (DateTime?)null,
                    RemovalOfConsentRegistrationAtUtc = PrivateDetailsAllowed(source) ? source.RemovalOfConsentRegistrationAtUtc : (DateTime?)null,
                },
                ParticipantDemographics = new ParticipantDemographicsResponse
                {
                    MobileNumber = PrivateDetailsAllowed(source) ? source.MobileNumber : null,
                    LandlineNumber = PrivateDetailsAllowed(source) ? source.LandlineNumber : null,
                    Address = PrivateDetailsAllowed(source) ? ParticipantAddressMapper.MapTo(source.Address) : null,
                    DateOfBirth = PrivateDetailsAllowed(source) ? source.DateOfBirth : (DateTime?)null,
                    SexRegisteredAtBirth = source.SexRegisteredAtBirth,
                    GenderIsSameAsSexRegisteredAtBirth = source.GenderIsSameAsSexRegisteredAtBirth,
                    EthnicGroup = source.EthnicGroup,
                    EthnicBackground = source.EthnicBackground,
                    Disability = source.Disability,
                    DisabilityDescription = source.DisabilityDescription,
                    HealthConditionInterests = source.HealthConditionInterests
                }
            };
        }
        
        public static ParticipantRegistrationResponse MapTo(ParticipantRegistration source)
        {
            return new ParticipantRegistrationResponse
            {
                StudyId = source.StudyId,
                SiteId = source.SiteId,
                ParticipantId = source.ParticipantId,
                ParticipantRegistrationStatus = source.ParticipantRegistrationStatus.ToString(),
                UpdatedAtUtc = source.UpdatedAtUtc,
                SubmittedAtUtc = source.SubmittedAtUtc,
                ParticipantDetails = new ParticipantDetailsResponse
                {
                    ParticipantId = source.ParticipantId,
                    Email = source.Email,
                    Firstname = source.Firstname,
                    Lastname = source.Lastname,
                    ConsentRegistration = source.ConsentRegistration,
                    ConsentRegistrationAtUtc = source.ConsentRegistrationAtUtc,
                    RemovalOfConsentRegistrationAtUtc = source.RemovalOfConsentRegistrationAtUtc
                },
                ParticipantDemographics = new ParticipantDemographicsResponse
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
                    HealthConditionInterests = source.HealthConditionInterests
                }
            };
        }
        
        private static bool PrivateDetailsAllowed(ParticipantRegistration source)
        {
            // Not for applied but only for 'Screening', 'Enrolled', 'Not enrolled', 'Withdrawn'
            return source.ParticipantRegistrationStatus == ParticipantRegistrationStatus.Screening ||
                   source.ParticipantRegistrationStatus == ParticipantRegistrationStatus.Enrolled ||
                   source.ParticipantRegistrationStatus == ParticipantRegistrationStatus.NotEnrolled ||
                   source.ParticipantRegistrationStatus == ParticipantRegistrationStatus.Withdrawn;
        }
    }
}