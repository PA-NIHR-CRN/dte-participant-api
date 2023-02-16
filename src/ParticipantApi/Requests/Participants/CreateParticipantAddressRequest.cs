namespace ParticipantApi.Requests.Participants
{
    public class CreateParticipantAddressRequest
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }
}