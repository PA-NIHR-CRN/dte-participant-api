using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Application.Contracts;
using Application.Settings;
using Domain.Entities.Participants;

namespace Infrastructure.Persistence
{
    public class ParticipantDynamoDbRepository : BaseDynamoDbRepository, IParticipantRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly IDynamoDBContext _context;
        private readonly DynamoDBOperationConfig _config;
        
        private static string ParticipantKey(string participantId) => $"PARTICIPANT#{participantId}";
        private static string ParticipantKey() => "PARTICIPANT#";
        // private static string DetailsKey() => "DETAILS#";
        // private static string DemographicsKey() => "DEMOGRAPHICS#";

        public ParticipantDynamoDbRepository(IAmazonDynamoDB client, IDynamoDBContext context, AwsSettings awsSettings) : base(client, context)
        {
            _client = client;
            _context = context;
            _config = new DynamoDBOperationConfig { OverrideTableName = awsSettings.ParticipantRegistrationDynamoDbTableName };
        }
        
        public async Task<ParticipantDetails> GetParticipantDetailsAsync(string participantId)
        {
            return await _context.LoadAsync<ParticipantDetails>(ParticipantKey(participantId), ParticipantKey(), _config);
        }
        public async Task<ParticipantDetails> GetParticipantDetailsByEmailAsync(string email)
        {
            return await _context.LoadAsync<ParticipantDetails>(email, _config);
        }

        public async Task<ParticipantDemographics> GetParticipantDemographicsAsync(string participantId)
        {
            return await _context.LoadAsync<ParticipantDemographics>(ParticipantKey(participantId), ParticipantKey(), _config);
        }

        public async Task CreateParticipantDetailsAsync(ParticipantDetails entity)
        {
            entity.Pk = ParticipantKey(entity.ParticipantId);
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

        public async Task UpdateParticipantDemographicsAsync(ParticipantDemographics entity)
        {
            await _context.SaveAsync(entity, _config);
        }
    }
}