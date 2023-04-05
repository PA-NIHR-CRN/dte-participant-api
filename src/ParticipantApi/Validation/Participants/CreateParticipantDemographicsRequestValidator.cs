using System;
using FluentValidation;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Validation.Participants
{
    public class CreateParticipantDemographicsRequestValidator : AbstractValidator<CreateParticipantDemographicsRequest>
    {
        public CreateParticipantDemographicsRequestValidator()
        {
            RuleFor(x => x.ParticipantId).NotEmpty();
            RuleFor(x => x.Address).SetValidator(new ParticipantAddressRequestValidator()).When(x => x.Address != null);
            RuleFor(x => x.SexRegisteredAtBirth).NotEmpty();
            RuleFor(x => x.EthnicGroup).NotEmpty();
            RuleFor(x => x.EthnicBackground).NotEmpty();
        }
    }
}