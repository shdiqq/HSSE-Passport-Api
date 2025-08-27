using System.Net;

namespace PTK.HSSEPassport.Api.Utilities.Base
{
    public class BaseJson<T>
    {
        public int Code { get; set; } = (int)HttpStatusCode.OK;
        public string? ErrorMsg { get; set; }
        public required T Data { get; set; }
    }
}
