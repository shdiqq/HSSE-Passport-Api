using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using PTK.HSSEPassport.Data.Service.Upload;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Impl
{
    public class FileServiceImpl : IFileService
	{
		private readonly HSSEPassportDbContext _db;
		private readonly IUploadService<FileUpload> _uploadService;

		public FileServiceImpl(HSSEPassportDbContext db, IUploadService<FileUpload> uploadService)
		{
			_db = db;
			_uploadService = uploadService;
		}

		public async Task<BaseReadFileModel> GetFile(int id, CancellationToken cancellationToken)
		{
			var file = await _db.FileUploads.SingleOrDefaultAsync(b => b.Id == id, cancellationToken) ?? throw new DataNotFoundException($"File with id {id} not found!");

			var a = await _uploadService.ReadFileByte(file);

			return new BaseReadFileModel 
			{
				FileContents = a.FileContents,
				ContentType = a.ContentType,
				FileName = a.FileName,
			};
		}

		public async Task<FileUpload> TestInput(IFormFile file, CancellationToken cancellationToken) 
		{
			var test = await _uploadService.UploadFileWithReturn("TEST","TEST",0,file, Flag:"TEST");

			return test;
		}

    }
}
