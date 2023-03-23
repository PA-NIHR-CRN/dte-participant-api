using System;
using System.Collections.Generic;

namespace ParticipantApi.Requests.Participants
{
    public class CreateParticipantDemographicsRequest
    {
        public string ParticipantId { get; set; }
        public string MobileNumber { get; set; }
        public string LandlineNumber { get; set; }
        public CreateParticipantAddressRequest Address { get; set; }
        public string SexRegisteredAtBirth { get; set; }
        public bool? GenderIsSameAsSexRegisteredAtBirth { get; set; }
        public string EthnicGroup { get; set; }
        public string EthnicBackground { get; set; }
        public bool? Disability { get; set; }
        public string DisabilityDescription { get; set; }
        public IEnumerable<string> HealthConditionInterests { get; set; }
    }
}