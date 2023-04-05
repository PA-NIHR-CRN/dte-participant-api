namespace Application.Settings
{
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";
        public string FromAddress { get; set; }
        public string WebAppBaseUrl { get; set; }
    }
}