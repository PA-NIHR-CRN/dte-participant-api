using Application.Models.Participants;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Mappers.Participants
{
    public static class ParticipantRequestMapper
    {
        public static ParticipantAddressModel MapTo(CreateParticipantAddressRequest source)
        {
            if (source == null) return null;
            
            return new ParticipantAddressModel
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Town = source.Town,
                Postcode = source.Postcode,
            };
        }
    }
}