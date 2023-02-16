using System;

namespace Application.Responses.V1.Participants
{
    public class ParticipantRegistrationResponse
    {
        public long StudyId { get; set; }
        public string SiteId { get; set; }
        public string ParticipantId { get; set; }
        public string ParticipantRegistrationStatus { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public DateTime SubmittedAtUtc { get; set; }
        public ParticipantDetailsResponse ParticipantDetails { get; set; }
        public ParticipantDemographicsResponse ParticipantDemographics { get; set; }
    }
}