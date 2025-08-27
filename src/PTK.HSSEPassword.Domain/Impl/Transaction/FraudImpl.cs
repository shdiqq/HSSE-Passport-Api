using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Transaction;
using PTK.HSSEPassport.Api.Domain.Models;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Data.Service.Upload;
using PTK.HSSEPassport.Utilities.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Praise.Domain.Impl.Praise.Transaction
{
    public class FraudImpl : IFraudService
    {
        private readonly HSSEPassportDbContext _db;
        private protected IUploadService<FileUpload> _uploadService;
        public FraudImpl(HSSEPassportDbContext db, IUploadService<FileUpload> uploadService)
        {
            _db = db;
            _uploadService = uploadService;
        }

        public async Task<BaseDTResponseModel<FraudDTModel>> DataTablePagination(FraudDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<FraudDTModel>
                {
                    Data = new List<FraudDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Fraud.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<FraudDTModel> raw = _db
                .Fraud
                .Include(b => b.Passport)
                .Include(b => b.Passport.User)
                .Include(b => b.FileFraud)
                .Where(b => (b.IsActive == true && b.IsDeleted == false)
                && (param.PassportId == null || b.Passport.Id == param.PassportId)
                && (string.IsNullOrEmpty(param.Email) || b.Passport.User.Email.Contains(param.Email))
                && (string.IsNullOrEmpty(param.UserName) || b.Passport.User.Name.Contains(param.UserName))
                && (string.IsNullOrEmpty(param.Type) || b.Type.Contains(param.Type))
                && (string.IsNullOrEmpty(param.Note) || b.Note.Contains(param.Note))
                && (string.IsNullOrEmpty(param.CreatedBy) || b.Passport.User.Name == param.CreatedBy))
                .Select(b => new FraudDTModel
                {
                    Id = b.Id,
                    UserId = b.Passport.User.Id,
                    Email = b.Passport.User.Email,
                    UserName = b.Passport.User.Name,
                    NIK = b.Passport.User.NIK,
                    Date = b.Date,
                    Type = b.Type,
                    Note = b.Note,
                    PassportId = b.Passport == null ? 0 : b.Passport.Id,
                    //PassportName = b.Passport == null ? 0 : b.Passport.Name,
                    FileUploadData = b.FileFraud == null ? null : new FileUploadModel { Id = b.FileFraud.Id, FileName = b.FileFraud.FileName },
                    CreatedDT = b.CreatedAt
                });


            IEnumerable<FraudDTModel> sorted;

            int totalCount = raw.Count();
            param.PageSize = param.PageSize < 0 ? totalCount : param.PageSize;

            if (string.IsNullOrWhiteSpace(param.ColumnIndex))
            {
                if (param.SortDirection == "desc")
                    sorted = await raw.OrderByDescending(b => b.CreatedDT).Skip(param.Skip).Take(param.PageSize).ToListAsync(cancellationToken);
                else
                    sorted = await raw.OrderBy(b => b.CreatedDT).Skip(param.Skip).Take(param.PageSize).ToListAsync(cancellationToken);
            }
            else
            {
                if (param.SortDirection == "desc")
                    sorted = raw.ToList().OrderByDescending(b => b.GetType().GetProperty(param.ColumnIndex).GetValue(b)).Skip(param.Skip).Take(param.PageSize);
                else
                    sorted = raw.ToList().OrderBy(b => b.GetType().GetProperty(param.ColumnIndex).GetValue(b)).Skip(param.Skip).Take(param.PageSize);
            }

            return new BaseDTResponseModel<FraudDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Fraud.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Fraud>> GetAll(CancellationToken cancellationToken)
          => await _db.Fraud.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Fraud> GetById(int id, CancellationToken cancellationToken)
            => await _db.Fraud.Include(b => b.Passport).SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Fraud with id '{id}' not found!");

        public async Task<Fraud> Create(FraudModel param, CancellationToken cancellationToken)
        {
            var data = new Fraud
            {
                Date = param.Date,
                Type = param.Type,
                Note = param.Note,
                Passport = await _db.Passport.SingleOrDefaultAsync(b => b.Id == param.PassportId, cancellationToken)
                    ?? throw new DataNotFoundException($"Passport with id '{param.PassportId}' not found!"),

                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.Fraud.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            if (param.FileUploadData != null)
            {
                data.FileFraud = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Fraud/Files/" + data.Id,
                    createBy: param.CreatedBy,
                    trxId: data.Id,
                    file: param.FileUploadData,
                    Flag: "Fraud",
                    autoRename: true,
                    isUpdate: false,
                    remark: "Fraud");

                _db.Fraud.Update(data);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return data;
        }

        public async Task<Fraud> Update(int id, FraudModel param, CancellationToken cancellationToken)
        {
            Fraud result = await
                _db.Fraud
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Fraud with id '{id}' not found!");

            result.Date = param.Date;
            result.Type = param.Type;
            result.Note = param.Note;
            result.Passport = await _db.Passport.SingleOrDefaultAsync(b => b.Id == param.PassportId, cancellationToken)
                ?? throw new DataNotFoundException($"Passport with id '{param.PassportId}' not found!");


            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            if (param.FileUploadData != null)
            {
                result.FileFraud = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Fraud/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.FileUploadData,
                    Flag: "Fraud",
                    autoRename: true,
                isUpdate: false,
                    remark: "Fraud");
            }

            _db.Fraud.Update(result);
            await _db.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<Fraud> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Fraud result = await _db
                .Fraud
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Fraud with id '{id}' not found!");

            result.IsDeleted = true;
            result.IsActive = false;
            result.UpdateBy = updatedBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

    }
}
