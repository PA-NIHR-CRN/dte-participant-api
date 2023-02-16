using System;

namespace Domain.Common
{
    public class AuditableEntity
    {
        public DateTime CreatedUtc { get; set; }
        public DateTime LastModifiedUtc { get; set; }
    }
}