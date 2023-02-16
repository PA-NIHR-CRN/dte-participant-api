namespace ParticipantApi.Requests.Participants
{
    public class CreateParticipantRegistrationRequest
    {
        public long StudyId { get; set; }
        public string SiteId { get; set; }
        public string ParticipantId { get; set; }
    }
}