using FluentValidation;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Validation.Participants
{
    public class ParticipantAddressRequestValidator : AbstractValidator<CreateParticipantAddressRequest>
    {
        public ParticipantAddressRequestValidator()
        {
            RuleFor(x => x.AddressLine1).NotEmpty();
            RuleFor(x => x.Town).NotEmpty();
        }
    }
}