using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Application.Content;
using Application.Contracts;
using Application.Settings;
using Dte.Common.Contracts;
using Domain.Entities.Participants;
using MediatR;
using Dte.Common.Exceptions;
using Dte.Common.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Participants.V1.Commands.Participants
{
    public class CreateParticipantDetailsNhsCommand : IRequest
    {
        private string ParticipantId { get; }
        private string Email { get; }
        private string Firstname { get; }
        private string Lastname { get; }
        private bool ConsentRegistration { get; }
        private string NhsId { get; }
        private string NhsNumber { get; }
        private DateTime? DateOfBirth { get; set; }

        public CreateParticipantDetailsNhsCommand(string participantId, string email, string firstname, string lastname,
            bool consentRegistration, string nhsId, DateTime? dateOfBirth, string nhsNumber
        )
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

        public class CreateParticipantDetailsNhsCommandHandler : IRequestHandler<CreateParticipantDetailsNhsCommand>
        {
            private readonly IAmazonCognitoIdentityProvider _provider;
            private readonly AwsSettings _awsSettings;
            private readonly IParticipantRepository _participantRepository;
            private readonly IClock _clock;
            private readonly IEmailService _emailService;
            private readonly EmailSettings _emailSettings;

            public CreateParticipantDetailsNhsCommandHandler(IParticipantRepository participantRepository, IClock clock,
                IAmazonCognitoIdentityProvider provider, AwsSettings awsSettings, EmailSettings emailSettings,
                IEmailService emailService)
            {
                _participantRepository = participantRepository;
                _clock = clock;
                _provider = provider;
                _awsSettings = awsSettings;
                _emailService = emailService;
                _emailSettings = emailSettings;
            }

            private async Task<string> AdminGetUserAsync(string email)
            {
                try
                {
                    var response = await _provider.AdminGetUserAsync(new AdminGetUserRequest
                    {
                        UserPoolId = _awsSettings.CognitoPoolId,
                        Username = email
                    });

                    return IsSuccessHttpStatusCode((int)response.HttpStatusCode)
                        ? response.Username
                        : null;
                }
                catch (UserNotFoundException)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            private async Task CreateUserAndDeactivateOldUserAsync(CreateParticipantDetailsNhsCommand request,
                ParticipantDetails participant)
            {
                var entity = new ParticipantDetails
                {
                    NhsId = request.NhsId,
                    NhsNumber = request.NhsNumber,
                    Email = request.Email,
                    ParticipantId = request.NhsId,
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    ConsentRegistration = participant.ConsentRegistration,
                    DateOfBirth = request.DateOfBirth,
                    ConsentRegistrationAtUtc = participant.ConsentRegistration ? _clock.Now() : (DateTime?)null,
                    RemovalOfConsentRegistrationAtUtc = (DateTime?)null,
                    CreatedAtUtc = _clock.Now(),
                };
                // check if demographic data is complete
                var demographics = await _participantRepository
                    .GetParticipantDemographicsAsync(participant.Pk.Replace("PARTICIPANT#", ""));
                if (demographics.HasDemographics)
                {
                    await _participantRepository.CreateParticipantDetailsAsync(entity);
                    await _participantRepository.AddDemographicsToNhsUserAsync(demographics, entity.NhsId);
                }
                else if (participant.ConsentRegistration)
                {
                    // create new user with consent and deactivate the old user
                    await _participantRepository.CreateParticipantDetailsAsync(entity);
                }

                //var baseUrl = _emailSettings.WebAppBaseUrl;
                //var htmlBody = EmailTemplate.GetHtmlTemplate().Replace("###TITLE_REPLACE1###",
                //        "New Be Part of Research Account")
                //    .Replace("###TEXT_REPLACE1###",
                //        $"Thank you for registering for Be Part of Research using your NHS login or through the NHS App. You will need to use the NHS login option on the <a href=\"{baseUrl}Participants/Options\">Be Part of Research</a> website each time you access your account.")
                //    .Replace("###TEXT_REPLACE2###",
                //        "By signing up, you are joining our community of amazing volunteers who are helping researchers to understand more about health and care conditions. Please visit the <a href=\"https://bepartofresearch.nihr.ac.uk/taking-part/how-to-take-part\">How to take part</a> section of the website to find out about other ways to take part in health and care research.")
                //    .Replace("###TEXT_REPLACE3###",
                //        "If you close your NHS login account, your Be Part of Research account will remain open and if you would also like to close your Be Part of Research account you will need to email bepartofresearch@nihr.ac.uk.")
                //    .Replace("###LINK_REPLACE###", "")
                //    .Replace("###LINK_DISPLAY_VALUE_REPLACE###", "block")
                //    .Replace("###TEXT_REPLACE4###",
                //        "Thank you for your ongoing commitment and support.")
                //    .Replace("###TEXT_REPLACE5###", "The Be Part of Research team");

                //await _emailService.SendEmailAsync(request.Email, "Be Part of Research", htmlBody);

                // still need to deactivate the old user in cognito
                var response = await _provider.AdminDisableUserAsync(new AdminDisableUserRequest
                    {
                        Username = participant.ParticipantId,
                        UserPoolId = _awsSettings.CognitoPoolId
                    }
                );

                if ((int)response.HttpStatusCode < 200 || (int)response.HttpStatusCode > 299)
                {
                    throw new AmazonCognitoIdentityProviderException($"Unable to disable user account: {response}");
                }
            }

            public async Task<Unit> Handle(CreateParticipantDetailsNhsCommand request,
                CancellationToken cancellationToken)
            {
                // check and see if the user matches an existing user in the database by NHS ID
                var participant = await _participantRepository.GetParticipantDetailsAsync(request.NhsId);
                if (participant != null)
                {
                    participant.Email = request.Email.ToLower();
                    participant.Firstname = request.Firstname;
                    participant.Lastname = request.Lastname;
                    participant.DateOfBirth = request.DateOfBirth;
                    participant.NhsNumber = request.NhsNumber;
                    participant.UpdatedAtUtc = _clock.Now();

                    await _participantRepository.UpdateParticipantDetailsAsync(participant);
                    return Unit.Value;
                }

                participant = await _participantRepository.GetParticipantDetailsByNhsNumberAsync(request.NhsNumber);
                // If participant is found, create new user and deactivate old user
                if (participant != null)
                {
                    await CreateUserAndDeactivateOldUserAsync(request, participant);
                }

                // Look up participant by email and date of birth
                var emailRequestId = await AdminGetUserAsync(request.Email);
                if (emailRequestId == null) return Unit.Value;
                participant = await _participantRepository.GetParticipantDetailsAsync(emailRequestId);
                if (participant == null) return Unit.Value;
                // check if string version of date of birth matches without time
                if (participant.DateOfBirth.HasValue && request.DateOfBirth.HasValue &&
                    participant.DateOfBirth.Value.Date ==
                    request.DateOfBirth.Value.Date)
                {
                    await CreateUserAndDeactivateOldUserAsync(request, participant);
                }
                else
                {
                    // pass back error message to be displayed
                    throw new ConflictException(ErrorCode.UnableToMatchAccounts);
                }

                return Unit.Value;
            }

            private static bool IsSuccessHttpStatusCode(int httpStatusCode) =>
                httpStatusCode >= StatusCodes.Status200OK &&
                httpStatusCode <
                StatusCodes.Status300MultipleChoices;
        }
    }
}