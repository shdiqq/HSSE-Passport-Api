using System.Net;

namespace PTK.HSSEPassport.Api.Utilities.Base
{
    public class ReturnJson
    {
        public string? ErrorMsg { get; set; }
        public dynamic? Payload { get; set; }
    }
}
