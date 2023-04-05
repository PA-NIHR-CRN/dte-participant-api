using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Application.Contracts;
using Application.Settings;
using Domain.Entities.Participants;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Persistence
{
    public class ParticipantDynamoDbRepository : BaseDynamoDbRepository, IParticipantRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly IDynamoDBContext _context;
        private readonly ILogger<ParticipantDynamoDbRepository> _logger;
        private readonly DynamoDBOperationConfig _config;

        private static string ParticipantKey(string participantId) => $"PARTICIPANT#{participantId}";

        private static string ParticipantKey() => "PARTICIPANT#";

        public ParticipantDynamoDbRepository(IAmazonDynamoDB client, IDynamoDBContext context, AwsSettings awsSettings,
            ILogger<ParticipantDynamoDbRepository> logger)
            : base(client, context)
        {
            _client = client;
            _context = context;
            this._logger = logger;
            _config = new DynamoDBOperationConfig
                { OverrideTableName = awsSettings.ParticipantRegistrationDynamoDbTableName };
        }

        public async Task<ParticipantDetails> GetParticipantDetailsAsync(string participantId)
        {
            return await _context.LoadAsync<ParticipantDetails>(ParticipantKey(participantId), ParticipantKey(),
                _config);
        }

        private async Task<ParticipantDetails> ScanForParticipantDetailsWithFilterAsync(
            string dbCol,
            AttributeValue filterValue)
        {
            var items = new List<Dictionary<string, AttributeValue>>();

            var scanRequestCount = 1;
            Dictionary<string, AttributeValue> lastKeyEvaluated = null;
            do
            {
                var request = new ScanRequest
                {
                    TableName = _config.OverrideTableName,
                    FilterExpression = $"#{dbCol} = :{dbCol}",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                        {
                            $"#{dbCol}", dbCol
                        }
                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { $":{dbCol}", filterValue }
                    },
                    ConsistentRead = true,
                    ExclusiveStartKey = lastKeyEvaluated,
                };

                _logger.LogInformation("request {scanRequestCount}: {request}", scanRequestCount,
                    JsonConvert.SerializeObject(request, Formatting.Indented));

                var response = await _client.ScanAsync(request);

                _logger.LogInformation("response {scanRequestCount}: {response}", scanRequestCount,
                    JsonConvert.SerializeObject(response, Formatting.Indented));

                lastKeyEvaluated = response.LastEvaluatedKey;

                items.AddRange(response.Items);

                scanRequestCount++;
            } while (lastKeyEvaluated != null && lastKeyEvaluated.Count != 0);

            _logger.LogInformation("items: {items}", JsonConvert.SerializeObject(items, Formatting.Indented));

            if (items.Count == 0)
            {
                return null;
            }

            var item = items.OrderByDescending(x => DateTime.Parse(x["CreatedAtUtc"].S)).First();

            // make item dictionary to an object
            var participantDetails = new ParticipantDetails
            {
                Pk = item["PK"].S,
                Sk = item["SK"].S,
                Email = item["Email"].S,
                Firstname = item["Firstname"].S,
                Lastname = item["Lastname"].S,
                ConsentRegistration = Convert.ToBoolean(Convert.ToInt16(item["ConsentRegistration"].N)),
                DateOfBirth = DateTime.Parse(item["DateOfBirth"].S),
            };

            if (item.TryGetValue("NhsId", out var nhsId))
            {
                participantDetails.NhsId = nhsId.S;
            }

            if (item.TryGetValue("NhsNumber", out var nhsNumber))
            {
                participantDetails.NhsNumber = nhsNumber.S;
            }

            if (item.TryGetValue("ParticipantId", out var participantId))
            {
                participantDetails.ParticipantId = participantId.S;
            }


            _logger.LogInformation("participantDetails: {participantDetails}",
                JsonConvert.SerializeObject(participantDetails));

            return participantDetails;
        }

        public async Task<ParticipantDetails> GetParticipantDetailsByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            email = email.ToLowerInvariant();

            var participantDetails =
                await ScanForParticipantDetailsWithFilterAsync("Email", new AttributeValue { S = email });

            return participantDetails;
        }

        public async Task<ParticipantDetails> GetParticipantDetailsByNhsNumberAsync(string nhsNumber)
        {
            if (string.IsNullOrEmpty(nhsNumber))
            {
                return null;
            }

            var participantDetails =
                await ScanForParticipantDetailsWithFilterAsync("NhsNumber", new AttributeValue { S = nhsNumber });

            _logger.LogInformation("participantDetails: {participantDetails}",
                JsonConvert.SerializeObject(participantDetails));

            return participantDetails;
        }

        public async Task<ParticipantDemographics> GetParticipantDemographicsAsync(string participantId)
        {
            return await _context.LoadAsync<ParticipantDemographics>(ParticipantKey(participantId), ParticipantKey(),
                _config);
        }

        public async Task CreateParticipantDetailsAsync(ParticipantDetails entity)
        {
            // TODO pull out this logic into a mapper
            entity.Pk = ParticipantKey(string.IsNullOrEmpty(entity.ParticipantId)
                ? entity.NhsId
                : entity.ParticipantId);
            entity.Sk = ParticipantKey();

            await _context.SaveAsync(entity, _config);
        }

        public async Task UpdateParticipantDetailsAsync(ParticipantDetails entity)
        {
            await _context.SaveAsync(entity, _config);
        }

        public async Task CreateParticipantDemographicsAsync(ParticipantDemographics entity)
        {
            entity.Pk = ParticipantKey(entity.ParticipantId);
            entity.Sk = ParticipantKey();

            await _context.SaveAsync(entity, _config);
        }

        public async Task AddDemographicsToNhsUserAsync(ParticipantDemographics entity, string nhsId)
        {
            entity.Pk = ParticipantKey(nhsId);
            entity.Sk = ParticipantKey();

            await _context.SaveAsync(entity, _config);
        }

        public async Task UpdateParticipantDemographicsAsync(ParticipantDemographics entity)
        {
            await _context.SaveAsync(entity, _config);
        }
    }
}
