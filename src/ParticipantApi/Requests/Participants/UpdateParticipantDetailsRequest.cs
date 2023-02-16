namespace ParticipantApi.Requests.Participants
{
    public class UpdateParticipantDetailsRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool ConsentRegistration { get; set; }
    }
}