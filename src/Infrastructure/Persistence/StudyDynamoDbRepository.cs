using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Application.Contracts;
using Application.Settings;
using Domain.Entities.Studies;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence
{
    public class StudyDynamoDbRepository : BaseDynamoDbRepository, IStudyRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly IDynamoDBContext _context;
        private readonly DynamoDBOperationConfig _config;
        
        private static string StudyKey(long studyId) => $"STUDY#{studyId}";
        private static string StudyKey() => "STUDY#";
        private static string SiteKey(long studyId) => $"STUDY#{studyId}#SITE#";
        private static string PreScreenerQuestionKey(long studyId, string reference, int version) => $"STUDY#{studyId}#PRESCREENERQUESTION#VERSION#{version}#REFERENCE#{reference}";

        public StudyDynamoDbRepository(IAmazonDynamoDB client, IDynamoDBContext context, IOptions<AwsSettings> awsSettings) : base(client, context)
        {
            _client = client;
            _context = context;
            _config = new DynamoDBOperationConfig { OverrideTableName = awsSettings.Value.StudyDynamoDbTableName };
        }

        public async Task<Study> GetStudyAsync(long studyId)
        {
            return await _context.LoadAsync<Study>(StudyKey(studyId), StudyKey(), _config);
        }
    }
}