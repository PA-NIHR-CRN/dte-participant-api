using Application.Models.Participants;
using Application.Responses.V1.Participants;
using Domain.Entities.Participants;

namespace Application.Mappings.Participants
{
    public static class ParticipantAddressMapper
    {
        public static ParticipantAddress MapTo(ParticipantAddressModel source)
        {
            if (source == null) return null;
            
            return new ParticipantAddress
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Town = source.Town,
                Postcode = source.Postcode,
            };
        }
        
        public static ParticipantAddressResponse MapTo(ParticipantAddress source)
        {
            if (source == null) return null;
            
            return new ParticipantAddressResponse
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