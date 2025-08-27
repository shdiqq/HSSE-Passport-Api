

using Microsoft.AspNetCore.Http;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces
{
    public interface IFileService
	{
		public Task<BaseReadFileModel> GetFile(int id,CancellationToken cancellationToken);
		public Task<FileUpload> TestInput(IFormFile file, CancellationToken cancellationToken);
	}
}
