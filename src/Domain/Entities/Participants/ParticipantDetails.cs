using System;
using Amazon.DynamoDBv2.DataModel;
using Domain.Converters;

namespace Domain.Entities.Participants
{
    public class ParticipantDetails
    {
        [DynamoDBHashKey("PK")] public string Pk { get; set; } // PARTICIPANT#1
        [DynamoDBRangeKey("SK")] public string Sk { get; set; } //PARTICIPANT#

        [DynamoDBProperty] public string ParticipantId { get; set; }
        [DynamoDBProperty] public string Email { get; set; }
        [DynamoDBProperty] public string Firstname { get; set; }
        [DynamoDBProperty] public string Lastname { get; set; }
        [DynamoDBProperty] public string NhsId { get; set; }
        [DynamoDBProperty] public bool ConsentRegistration { get; set; }
        [DynamoDBProperty] public DateTime? DateOfBirth { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? ConsentRegistrationAtUtc { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? RemovalOfConsentRegistrationAtUtc { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime? UpdatedAtUtc { get; set; }
        [DynamoDBProperty(typeof(DateTimeUtcConverter))] public DateTime CreatedAtUtc { get; set; }
        
        public bool HasDemographics => DateOfBirth.HasValue;
    }
}