using System;

namespace Application.Responses.V1.Participants
{
    public class AdminGetUserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}