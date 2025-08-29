namespace PTK.HSSEPassport.Api.Domain.Interfaces
{
    public interface IAppLogService
    {
        public Task SaveAppLog(
            string controllerName,
            string methodName,
            string userName,
            string trxId,
            string status,
            CancellationToken cancellationToken,
            string remark = null,
            string errorMessage = null,
            string info = null);
    }
}
