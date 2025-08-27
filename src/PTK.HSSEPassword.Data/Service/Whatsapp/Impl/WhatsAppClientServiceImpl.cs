using PTK.HSSEPassport.Data.Service.Whatsapp.Models;

namespace PTK.HSSEPassport.Data.Service.Whatsapp.Impl
{
    public class WhatsAppClientServiceImpl : IWhatsAppClientService
    {
        private readonly string CLIENT_NAME = "WA.SERVICE";

        private readonly IHttpClientFactory _httpClientFactory;

        public WhatsAppClientServiceImpl(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private string GenerateEndPoint()
        {
            return "/Api/WhatsApp/Send";
        }

        public async Task Send(WhatsAppClientSendRequestModel d, CancellationToken c)
        {
            HttpResponseMessage httpResponseMessage = await _httpClientFactory.CreateClient(CLIENT_NAME).PostAsJsonAsync(GenerateEndPoint(), d, c);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return;
            }

            throw new Exception($"Server error when call '{httpResponseMessage.RequestMessage.RequestUri}' with code {httpResponseMessage.StatusCode}");
        }
    }
}
