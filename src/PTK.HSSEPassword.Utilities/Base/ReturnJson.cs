using System.Net;

namespace PTK.HSSEPassport.Api.Utilities.Base
{
    public class ReturnJson
    {
        public string? Token { get; set; }
        public string? TokenType { get; set; }
        public string? ErrorMsg { get; set; }
        public dynamic? Payload { get; set; }
    }
}
