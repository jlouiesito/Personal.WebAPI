namespace Personal.WebAPI.Configurations
{
    public class SmtpConfig
    {
        public string loginUrl { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
    }
}
