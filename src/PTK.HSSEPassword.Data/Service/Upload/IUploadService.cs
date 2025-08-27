using Microsoft.AspNetCore.Mvc;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Data.Service.Upload
{
    public interface IUploadService<T> where T : BaseDaoFile
    {
        Task<T> UploadFileWithReturn(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, bool autoRename = false, string remark = null, bool isCompress = false);
        Task<string> GetFilePath(int id);
        Task DeleteFile(T oldFile, string updateBy);
        Task<BaseReadFileModel> ReadFileByte(int id);
        Task<BaseReadFileModel> ReadFileByte(T file);
    }
}
