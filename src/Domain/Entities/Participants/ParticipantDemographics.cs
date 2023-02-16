using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Domain.Converters;

namespace Domain.Entities.Participants
{
    public class ParticipantDemographics
    {
        [DynamoDBHashKey("PK")] public string Pk { get; set; } // PARTICIPANT#1
        [DynamoDBRangeKey("SK")] public string Sk { get; set; } //PARTICIPANT#

        [DynamoDBProperty] public string ParticipantId { get; set; }
        [DynamoDBProperty] public string MobileNumber { get; set; }
        [DynamoDBProperty] public string LandlineNumber { get; set; }
        [DynamoDBProperty] public ParticipantAddress Address { get; set; }
        [DynamoDBProperty] public DateTime? DateOfBirth { get; set; }
        [DynamoDBProperty] public string SexRegisteredAtBirth { get; set; }
        [DynamoDBProperty] public bool? GenderIsSameAsSexRegisteredAtBirth { get; set; }
        [DynamoDBProperty] public string EthnicGroup { get; set; }
        [DynamoDBProperty] public string EthnicBackground { get; set; }
        [DynamoDBProperty] public bool? Disability { get; set; }
        [DynamoDBProperty] public string DisabilityDescription { get; set; }
        [DynamoDBProperty] public List<string> HealthConditionInterests { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? UpdatedAtUtc { get; set; }
        
        public bool HasDemographics => DateOfBirth.HasValue;
    }
}