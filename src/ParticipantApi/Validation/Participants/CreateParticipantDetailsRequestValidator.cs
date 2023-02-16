using FluentValidation;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Validation.Participants
{
    public class CreateParticipantDetailsRequestValidator : AbstractValidator<CreateParticipantDetailsRequest>
    {
        public CreateParticipantDetailsRequestValidator()
        {
            RuleFor(x => x.ParticipantId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Firstname).NotEmpty();
            RuleFor(x => x.Lastname).NotEmpty();
        }
    }
}