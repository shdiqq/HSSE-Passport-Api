using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Models;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Data.Service.Upload;
using PTK.HSSEPassport.Utilities.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Praise.Domain.Impl.Praise.MasterData
{
    public class PertaminaImpl : IPertaminaService
    {
        private readonly HSSEPassportDbContext _db;
        private protected IUploadService<FileUpload> _uploadService;
        public PertaminaImpl(HSSEPassportDbContext db, IUploadService<FileUpload> uploadService)
        {
            _db = db;
            _uploadService = uploadService;
        }

        public async Task<BaseDTResponseModel<PertaminaDTModel>> DataTablePagination(PertaminaDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<PertaminaDTModel>
                {
                    Data = new List<PertaminaDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Pertamina.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<PertaminaDTModel> raw = _db
                .Pertamina
                .Include(b => b.Logo)
                .Include(b => b.OfficialSign)
                .Where(b => (b.IsActive == true && b.IsDeleted == false) 
                && (string.IsNullOrEmpty(param.Code) || b.Code.Contains(param.Code))
                && (string.IsNullOrEmpty(param.Name) || b.Name.Contains(param.Name))
                && (string.IsNullOrEmpty(param.OfficialName) || b.OfficialName.Contains(param.OfficialName))
                && (string.IsNullOrEmpty(param.OfficialPosition) || b.OfficialPosition.Contains(param.OfficialPosition))
                )
                .Select(b => new PertaminaDTModel
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    OfficialName = b.OfficialName,
                    OfficialPosition = b.OfficialPosition,
                    LogoData = b.Logo == null ? null : new FileUploadModel { Id = b.Logo.Id, FileName = b.Logo.FileName },
                    OfficialSignData = b.OfficialSign == null ? null : new FileUploadModel { Id = b.OfficialSign.Id, FileName = b.OfficialSign.FileName },
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<PertaminaDTModel> sorted;

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

            return new BaseDTResponseModel<PertaminaDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Pertamina.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Pertamina>> GetAll(CancellationToken cancellationToken)
          => await _db.Pertamina.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Pertamina> GetById(int id, CancellationToken cancellationToken)
            => await _db.Pertamina.SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Pertamina with id '{id}' not found!");

        public async Task<Pertamina> Create(PertaminaModel param, CancellationToken cancellationToken)
        {
            var data = new Pertamina
            {
                Code = param.Code,
                Name = param.Name,
                OfficialName = param.OfficialName,
                OfficialPosition = param.OfficialPosition,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);


            if (param.LogoData != null)
            {
                data.Logo = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Pertamina/Files/" + data.Id,
                    createBy: param.CreatedBy,
                    trxId: data.Id,
                    file: param.LogoData,
                    Flag: "Logo",
                    autoRename: true,
                    isUpdate: false,
                    remark: "Logo");

                _db.Pertamina.Update(data);
                await _db.SaveChangesAsync(cancellationToken);
            }

            if (param.OfficialSignData != null)
            {
                data.OfficialSign = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Pertamina/Files/" + data.Id,
                    createBy: param.CreatedBy,
                    trxId: data.Id,
                    file: param.OfficialSignData,
                    Flag: "OfficialSign",
                    autoRename: true,
                    isUpdate: false,
                    remark: "OfficialSign");

                _db.Pertamina.Update(data);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return data;
        }

        public async Task<Pertamina> Update(int id, PertaminaModel param, CancellationToken cancellationToken)
        {
            Pertamina result = await
                _db.Pertamina
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Pertamina with id '{id}' not found!");

            result.Code = param.Code;
            result.Name = param.Name;
            result.OfficialName = param.OfficialName;
            result.OfficialPosition = param.OfficialPosition;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);


            if (param.LogoData != null)
            {
                result.Logo = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Pertamina/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.LogoData,
                    Flag: "Logo",
                    autoRename: true,
                isUpdate: false,
                    remark: "Logo");

                _db.Pertamina.Update(result);
                await _db.SaveChangesAsync(cancellationToken);
            }

            if (param.OfficialSignData != null)
            {
                result.OfficialSign = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Pertamina/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.OfficialSignData,
                    Flag: "OfficialSign",
                    autoRename: true,
                    isUpdate: false,
                    remark: "OfficialSign");

                _db.Pertamina.Update(result);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        public async Task<Pertamina> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Pertamina result = await _db
                .Pertamina
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Pertamina with id '{id}' not found!");

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
