using FluentValidation;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Validation.Participants
{
    public class UpdateParticipantDetailsRequestValidator : AbstractValidator<UpdateParticipantDetailsRequest>
    {
        public UpdateParticipantDetailsRequestValidator()
        {
            RuleFor(x => x.Firstname).NotEmpty();
            RuleFor(x => x.Lastname).NotEmpty();
        }
    }
}