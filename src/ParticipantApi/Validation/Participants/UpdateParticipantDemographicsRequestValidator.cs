using System;
using FluentValidation;
using ParticipantApi.Requests.Participants;

namespace ParticipantApi.Validation.Participants
{
    public class UpdateParticipantDemographicsRequestValidator : AbstractValidator<UpdateParticipantDemographicsRequest>
    {
        public UpdateParticipantDemographicsRequestValidator()
        {
            RuleFor(x => x.Address).SetValidator(new ParticipantAddressRequestValidator()).When(x => x.Address != null);
            RuleFor(x => x.DateOfBirth).NotEmpty().GreaterThan(DateTime.MinValue);
            RuleFor(x => x.SexRegisteredAtBirth).NotEmpty();
            RuleFor(x => x.EthnicGroup).NotEmpty();
            RuleFor(x => x.EthnicBackground).NotEmpty();
        }
    }
}