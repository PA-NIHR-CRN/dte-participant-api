using FluentValidation;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Validation.Participants
{
    public class UpdateParticipantEmailRequestValidator : AbstractValidator<UpdateParticipantEmailRequest>
    {
        public UpdateParticipantEmailRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}