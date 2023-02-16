using System;

namespace Application.Responses.V1.Participants
{
    public class ParticipantDetailsResponse
    {
        public string ParticipantId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool ConsentRegistration { get; set; }
        public DateTime? ConsentRegistrationAtUtc { get; set; }
        public DateTime? RemovalOfConsentRegistrationAtUtc { get; set; }
        public bool HasDemographics { get; set; }
        public string NhsId { get; set; }
    }
}