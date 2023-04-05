namespace Domain.Entities.Participants
{
    public class ParticipantAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }

        public void Clear()
        {
            AddressLine1 = null;
            AddressLine2 = null;
            AddressLine3 = null;
            AddressLine4 = null;
            Town = null;
        }
    }
}