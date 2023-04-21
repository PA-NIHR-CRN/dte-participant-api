using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Application.Contracts;
using Application.Settings;
using Domain.Entities.ParticipantRegistrations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence
{
    public class ParticipantRegistrationDynamoDbRepository : BaseDynamoDbRepository, IParticipantRegistrationRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly IDynamoDBContext _context;
        private readonly DynamoDBOperationConfig _config;
        
        private static string StudyKey(long studyId) => $"STUDY#{studyId}";
        private static string StudySiteParticipantsKey(long studyId, string siteId) => $"STUDY#{studyId}#SITE#{siteId}#PARTICIPANT#";
        private static string StudySiteParticipantKeys(long studyId, string siteId, string participantId) => $"STUDY#{studyId}#SITE#{siteId}#PARTICIPANT#{participantId}";

        public ParticipantRegistrationDynamoDbRepository(IAmazonDynamoDB client, IDynamoDBContext context, IOptions<AwsSettings> awsSettings) : base(client, context)
        {
            _client = client;
            _context = context;
            _config = new DynamoDBOperationConfig { OverrideTableName = awsSettings.Value.ParticipantRegistrationDynamoDbTableName };
        }

        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudySiteAsync(long studyId, string siteId)
        {
            var items = await _context
                .QueryAsync<ParticipantRegistration>
                (
                    StudyKey(studyId), QueryOperator.BeginsWith, new[] { StudySiteParticipantsKey(studyId, siteId) }, 
                    _config
                )
                .GetRemainingAsync();

            return items;
        }

        // TODO - return paged data for ALL queries
        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudyAsync(long studyId, string participantId)
        {
            var items = await _context.QueryAsync<ParticipantRegistration>
            (
                StudyKey(studyId), QueryOperator.Equal, new[] { participantId }, new DynamoDBOperationConfig
                {
                    OverrideTableName = _config.OverrideTableName,
                    IndexName = "StudyParticipantIndex"
                }
            )
            .GetRemainingAsync();

            return items;
        }

        public async Task<ParticipantRegistration> GetParticipantByStudySiteAsync(long studyId, string siteId, string participantId)
        {
            var item = await _context.LoadAsync<ParticipantRegistration>
            (
                StudyKey(studyId), StudySiteParticipantKeys(studyId, siteId, participantId),
                _config
            );

            return item;
        }

        // TODO - return paged data for ALL queries
        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudySiteStatusAsync(long studyId, string siteId, ParticipantRegistrationStatus participantRegistrationStatus)
        {
            var items = await _context.QueryAsync<ParticipantRegistration>
                (
                    participantRegistrationStatus, QueryOperator.BeginsWith, new object [] { StudySiteParticipantsKey(studyId, siteId) }, new DynamoDBOperationConfig
                    {
                        OverrideTableName = _config.OverrideTableName,
                        IndexName = "ParticipantRegistrationStatusIndex"
                    }
                )
                .GetRemainingAsync();

            return items;
        }

        // TODO - return paged data for ALL queries
        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantRegistrationsStatusByStudy(long studyId, ParticipantRegistrationStatus participantRegistrationStatus)
        {
            var items = await _context.QueryAsync<ParticipantRegistration>
                (
                    StudyKey(studyId), QueryOperator.Equal, new object [] { participantRegistrationStatus }, new DynamoDBOperationConfig
                    {
                        OverrideTableName = _config.OverrideTableName,
                        IndexName = "StudyStatusIndex"
                    }
                )
                .GetRemainingAsync();

            return items;
        }

        public async Task CreateParticipantRegistrationAsync(ParticipantRegistration entity)
        {
            entity.Pk = StudyKey(entity.StudyId);
            entity.Sk = StudySiteParticipantKeys(entity.StudyId, entity.SiteId, entity.ParticipantId);
            
            await _context.SaveAsync(entity, _config);
        }

        public async Task SaveParticipantRegistrationAsync(ParticipantRegistration entity)
        {
            await _context.SaveAsync(entity, _config);
        }
    }
}