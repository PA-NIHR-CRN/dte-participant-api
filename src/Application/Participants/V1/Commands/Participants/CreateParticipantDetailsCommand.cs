using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Dte.Common.Contracts;
using Domain.Entities.Participants;
using MediatR;

namespace Application.Participants.V1.Commands.Participants
{
    public class CreateParticipantDetailsCommand : IRequest
    {
        public string ParticipantId { get; }
        public string Email { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public bool ConsentRegistration { get; }
        public string NhsId { get; }
        public string NhsNumber { get; }
        public DateTime? DateOfBirth { get; set; }

        public CreateParticipantDetailsCommand(string participantId, string email, string firstname, string lastname,
            bool consentRegistration, string nhsId, DateTime? dateOfBirth, string nhsNumber)
        {
            ParticipantId = participantId;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            ConsentRegistration = consentRegistration;
            NhsId = nhsId;
            NhsNumber = nhsNumber;
            DateOfBirth = dateOfBirth;
        }

        public class CreateParticipantDetailsCommandHandler : IRequestHandler<CreateParticipantDetailsCommand>
        {
            private readonly IParticipantRepository _participantRepository;
            private readonly IClock _clock;

            public CreateParticipantDetailsCommandHandler(IParticipantRepository participantRepository, IClock clock)
            {
                _participantRepository = participantRepository;
                _clock = clock;
            }

            public async Task<Unit> Handle(CreateParticipantDetailsCommand request, CancellationToken cancellationToken)
            {
                var entity = new ParticipantDetails
                {
                    NhsId = request.NhsId,
                    NhsNumber = request.NhsNumber,
                    ParticipantId = request.ParticipantId,
                    Email = request.Email.ToLower(),
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    ConsentRegistration = request.ConsentRegistration,
                    DateOfBirth = request.DateOfBirth,
                    ConsentRegistrationAtUtc = request.ConsentRegistration ? _clock.Now() : (DateTime?)null,
                    RemovalOfConsentRegistrationAtUtc = (DateTime?)null,
                    CreatedAtUtc = _clock.Now(),
                };

                await _participantRepository.CreateParticipantDetailsAsync(entity);

                return Unit.Value;
            }
        }
    }
}