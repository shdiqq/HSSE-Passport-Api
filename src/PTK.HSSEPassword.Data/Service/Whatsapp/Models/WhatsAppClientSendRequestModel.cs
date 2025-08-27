namespace PTK.HSSEPassport.Data.Service.Whatsapp.Models
{
    public class WhatsAppClientSendRequestModel
    {
        public string? Application { get; set; }

        public string? Module { get; set; }

        public string? Text { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsSkipProcess { get; set; } = true;

    }
}
