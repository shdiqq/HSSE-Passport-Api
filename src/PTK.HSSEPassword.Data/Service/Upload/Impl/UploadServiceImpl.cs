using ImageMagick;
using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Data.Service.Upload.Impl
{
    public class UploadServiceImpl<T, D> : IUploadService<T> where T : BaseDaoFile, new() where D : DbContext
    {
        private readonly D _db;

        public UploadServiceImpl(D db)
        {
            _db = db;
        }

        private byte[] CompressImage(IFormFile img)
        {
            byte[] array = null;
            using MagickImage magickImage = new MagickImage(img.OpenReadStream());
            magickImage.Quality = 50;
            return magickImage.ToByteArray();
        }

        private string CreatePath(string path)
        {
            return Path.Combine(path, DateTime.UtcNow.ToString("yyyyMMdd/"));
        }

        public async Task DeleteFile(T oldFile, string updateBy)
        {
            if (oldFile == null)
            {
                return;
            }

            string deletePath = GeneralConstant.ReplaceDeletedPath(oldFile.FilePath);

            if (!Directory.Exists(GeneralConstant.URL_UPLOAD + deletePath))
            {
                Directory.CreateDirectory(GeneralConstant.URL_UPLOAD + deletePath);
            };

            string fileFrom = GeneralConstant.URL_UPLOAD + Path.Combine(oldFile.FilePath, oldFile.FileName);
            string fileTarget = GeneralConstant.URL_UPLOAD + GeneralConstant.CreateDeletedPath(Path.Combine(deletePath, oldFile.FileName));

            if (File.Exists(fileFrom) && System.IO.File.Exists(fileTarget))
            {
                File.Delete(fileTarget);
            }

            if (File.Exists(fileFrom))
                File.Move(fileFrom, fileTarget);

            // Update old file data
            //uploadFileOld.FilePath = PrideConstant.URL_DOWNLOAD_UPLOAD + "/upload/" + path + "DeleteFiles/" + trxId + "/";
            oldFile.FilePath = deletePath;
            oldFile.IsDeleted = true;
            oldFile.UpdateBy = updateBy;
            oldFile.UpdateDt = DateTime.Now;
            _db.Set<T>().Update(oldFile);
            await _db.SaveChangesAsync();
        }

        public async Task<T> UploadFileWithReturn(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string? Flag = null, bool autoRename = false, string? remark = null, bool isCompress = false)
        {
            if (file != null)
            {
                string filePath = GeneralConstant.CreateUploadPathNew(path);
                //string filePath = @"\\ptmkpshare2.pertamina.com\ptmpisappprd01\HSSEPassportPTKDev\upload\"+path\;
                string filePathView = GeneralConstant.CreateUploadPathView(path);
                string fileName = file.FileName.ToLower();
                byte[]? compresed = null;
                if (isCompress)
                {
                    compresed = CompressImage(file);
                }

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                T uploadFile = new T
                {
                    BucketName = "uploaded",
                    FilePath = filePathView,
                    FileName = fileName,
                    ContentType = file.ContentType,
                    Length = (isCompress ? compresed.Length : file.Length),
                    TrxId = trxId,
                    CreatedBy = createBy,
                    Flag = Flag,
                    Remark = remark
                };

                // Delete File First while Update Action
                if (isUpdate)
                {
                    await DeleteFile(await _db.Set<T>().SingleOrDefaultAsync((T b) => b.TrxId == trxId && b.Flag == Flag && b.Remark == remark && b.IsDeleted != true), createBy);
                }

                await _db.Set<T>().AddAsync(uploadFile);
                await _db.SaveChangesAsync();

                if (autoRename)
                {
                    uploadFile.FileName = $"{uploadFile.Id}-{uploadFile.FileName}";
                    _ = uploadFile.FileName;
                    _db.Set<T>().Update(uploadFile);
                }

                using (var stream = new FileStream(Path.Combine(filePath, uploadFile.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    stream.Flush();
                }
                await _db.SaveChangesAsync();
                return uploadFile;
            }
            return null;
        }

        public async Task<string> GetFilePath(int id)
        {
            T val = await _db.Set<T>().SingleOrDefaultAsync((T b) => b.Id == id);
            if (val == null) 
            {
                throw new FileNotFoundException();
            }
            string result = GeneralConstant.URL_UPLOAD + val.FilePath + val.FileName;
            if (!File.Exists(result))
                throw new FileNotFoundException();
            return result;
        }

        public async Task<BaseReadFileModel> ReadFileByte(int id)
        {
            T file = await _db.Set<T>().SingleOrDefaultAsync((T b) => b.Id == id && b.IsDeleted != true);

            if (file == null) 
            {
                throw new FileNotFoundException();
            }
            return await ReadFileByte(file);
        }

        public async Task<BaseReadFileModel> ReadFileByte(T file)
        {

            string filePath = file != null ? (GeneralConstant.URL_UPLOAD + file.FilePath + file.FileName) : throw new FileNotFoundException();

            bool isFileExists = File.Exists(filePath);

            byte[] fileContents = isFileExists ? await File.ReadAllBytesAsync(filePath) : throw new FileNotFoundException();

            return new BaseReadFileModel
            {
                ContentType = file.ContentType,
                FileContents = fileContents,
                FileName = file.FileName
            };
        }

    }
}
