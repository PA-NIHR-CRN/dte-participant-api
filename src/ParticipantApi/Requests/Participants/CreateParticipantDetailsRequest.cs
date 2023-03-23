using System;

namespace ParticipantApi.Requests.Participants
{
    public class CreateParticipantDetailsRequest
    {
        public string ParticipantId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool ConsentRegistration { get; set; }
        public string NhsId { get; set; }
        public string NhsNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}