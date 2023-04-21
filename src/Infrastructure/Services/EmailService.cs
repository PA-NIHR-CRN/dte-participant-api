using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Application.Contracts;
using Application.Settings;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IAmazonSimpleEmailService _client;

        public EmailService(EmailSettings emailSettings, IAmazonSimpleEmailService client)
        {
            _emailSettings = emailSettings;
            _client = client;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var from = _emailSettings.FromAddress;
            var request = new SendEmailRequest
            {
                Source = from,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { to }
                },
                Message = new Message
                {
                    Subject = new Amazon.SimpleEmail.Model.Content(subject),
                    Body = new Body
                    {
                        Html = new Amazon.SimpleEmail.Model.Content(body)
                    } 
                }
            };

            await _client.SendEmailAsync(request);
        }
    }
}