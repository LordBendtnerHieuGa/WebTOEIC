namespace WebToeic.WebAppMVC.Data.Entities
{
    public class SMTPConfig
    {
        public string SenderAddress { get; set; }
        public string SenderDisplayName { get; set; }
        public string Password { get; set; }
        public string host { get; set; }
        public string Port { get; set; }
        public string EnableSSL { get; set; }
        public string UseDefaultCredentials { get; set; }
        public string IsBodyHTML { get; set; }
    }
}
