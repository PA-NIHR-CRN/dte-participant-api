using System;
using FluentValidation;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Validation.Participants
{
    public class CreateParticipantDetailsRequestValidator : AbstractValidator<CreateParticipantDetailsRequest>
    {
        public CreateParticipantDetailsRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Firstname).NotEmpty();
            RuleFor(x => x.Lastname).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty().GreaterThan(DateTime.MinValue);
            RuleFor(x => x.NhsId).NotEmpty().When(x => string.IsNullOrEmpty(x.ParticipantId));
            RuleFor(x => x.ParticipantId).NotEmpty().When(x => string.IsNullOrEmpty(x.NhsId));
        }
    }
}