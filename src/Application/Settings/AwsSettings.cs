namespace Application.Settings
{
    public class AwsSettings
    {
        public static string SectionName => "AwsSettings";
        public string StudyDynamoDbTableName { get; set; }
        public string StudyRegistrationDynamoDbTableName { get; set; }
        public string ParticipantRegistrationDynamoDbTableName { get; set; }
        public string ResearcherStudyDynamoDbTableName { get; set; }
        public string ServiceUrl { get; set; }
    }
}