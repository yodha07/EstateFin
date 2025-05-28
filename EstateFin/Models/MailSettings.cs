namespace EstateFin.Models
{
    public class MailSettings
    {
        public string EmailId { get; set; } = "adityaalt218@gmail.com";
        public string DisplayName { get; set; } = "Estate Fin";
        public string UserName { get; set; } = "adityaalt218@gmail.com";
        public string Password { get; set; } = "gbqxshitgwjhenwm";
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool UseSSL
        {
            get; set;
        } = true;
    }
}
