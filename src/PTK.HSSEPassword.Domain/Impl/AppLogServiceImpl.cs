using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces;

namespace PTK.HSSEPassport.Api.Domain.Impl
{
	public class AppLogServiceImpl : IAppLogService
	{
		private readonly HSSEPassportDbContext _db;

		public AppLogServiceImpl(HSSEPassportDbContext db)
		{
			_db = db;
		}

		public async Task SaveAppLog(string controllerName, string methodName, string userName, string trxId, string status, CancellationToken cancellationToken, string remark = null, string errorMessage = null, string info = null)
		{
			await _db.ActivityLogs.AddAsync(new ActivityLog
			{
				Controller = controllerName,
				Method = methodName,
				TrxId = trxId,
				UserName = userName,
				Status = status,
				ErrorMessage = errorMessage,
				Remark = remark,
				Info = info
			}, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
		}
	}
}
