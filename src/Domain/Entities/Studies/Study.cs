using System;
using Amazon.DynamoDBv2.DataModel;
using Domain.Converters;

namespace Domain.Entities.Studies
{
    public class Study
    {
        [DynamoDBHashKey("PK")] public string Pk { get; set; }
        [DynamoDBRangeKey("SK")] public string Sk { get; set; }

        [DynamoDBProperty] public long StudyId { get; set; }
        [DynamoDBProperty] public long CpmsId { get; set; }
        [DynamoDBProperty] public string IsrctnId { get; set; }
        [DynamoDBProperty] public string Title { get; set; }
        [DynamoDBProperty] public string ShortName { get; set; }
        [DynamoDBProperty] public string About { get; set; }
        [DynamoDBProperty] public string HowHelp { get; set; }
        [DynamoDBProperty] public string WhenNeeded { get; set; }
        [DynamoDBProperty] public string StudyQuestionnaireLink { get; set; }
        
        // Lead Researcher - TODO: This needs to be an entry in a table along with other researchers that are part of this study
        [DynamoDBProperty] public string LeadResearcherId { get; set; }
        [DynamoDBProperty] public string LeadResearcherFirstname { get; set; }
        [DynamoDBProperty] public string LeadResearcherLastname { get; set; }
        [DynamoDBProperty] public string LeadResearcherEmail { get; set; }
        
        [DynamoDBProperty] public StudyStatus Status { get; set; }

        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime CreatedAtUtc { get; set; }
        [DynamoDBProperty] public string CreatedById { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? UpdatedAtUtc { get; set; }
        [DynamoDBProperty] public string UpdatedById { get; set; }
    }
}