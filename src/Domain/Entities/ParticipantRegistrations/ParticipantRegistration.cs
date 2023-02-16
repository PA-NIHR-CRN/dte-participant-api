using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Domain.Converters;
using Domain.Entities.Participants;

namespace Domain.Entities.ParticipantRegistrations
{
    public class ParticipantRegistration
    {
        [DynamoDBHashKey("PK")] public string Pk { get; set; } // STUDY#1
        [DynamoDBRangeKey("SK"), DynamoDBGlobalSecondaryIndexRangeKey] public string Sk { get; set; } // STUDY#12345#SITE#{GUID}#PARTICIPANT#1

        [DynamoDBProperty] public long StudyId { get; set; }
        [DynamoDBProperty] public string SiteId { get; set; }
        [DynamoDBGlobalSecondaryIndexRangeKey] public string ParticipantId { get; set; }

        // participant details
        [DynamoDBProperty] public string Email { get; set; }
        [DynamoDBProperty] public string Firstname { get; set; }
        [DynamoDBProperty] public string Lastname { get; set; }
        [DynamoDBProperty] public bool ConsentRegistration { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? ConsentRegistrationAtUtc { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? RemovalOfConsentRegistrationAtUtc { get; set; }

        // participant demographics
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

        [DynamoDBGlobalSecondaryIndexHashKey, DynamoDBGlobalSecondaryIndexRangeKey] public ParticipantRegistrationStatus ParticipantRegistrationStatus { get; set; }
        
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? UpdatedAtUtc { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime SubmittedAtUtc { get; set; }
    }
}