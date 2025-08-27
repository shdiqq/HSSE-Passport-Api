using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Data.Service.Upload;
using PTK.HSSEPassport.Utilities.Base;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Praise.Domain.Impl.Praise.MasterData
{
    public class UserImpl : IUserService
    {
        private readonly HSSEPassportDbContext _db;
        private protected IUploadService<FileUpload> _uploadService;
        public UserImpl(HSSEPassportDbContext db, IUploadService<FileUpload> uploadService = null)
        {
            _db = db;
            _uploadService = uploadService;
        }

        public async Task<BaseDTResponseModel<UserDTModel>> DataTablePagination(UserDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<UserDTModel>
                {
                    Data = new List<UserDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.User.Where(b => b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<UserDTModel> raw = _db
                .User
                .Include(b => b.Pertamina)
                .Where(b => (b.IsDeleted == false)
                && ((param.PertaminaId == null || param.PertaminaId == 0) || b.Pertamina.Id == param.PertaminaId)
                && (string.IsNullOrEmpty(param.Email) || b.Email.Contains(param.Email))
                && (string.IsNullOrEmpty(param.Name) || b.Name.Contains(param.Name))
                && (string.IsNullOrEmpty(param.Telp) || b.Telp.Contains(param.Telp))
                && (string.IsNullOrEmpty(param.NIP) || b.NIP.Contains(param.NIP))
                && (string.IsNullOrEmpty(param.NIK) || b.NIK.Contains(param.NIK))
                )
                .Select(b => new UserDTModel
                {
                    Id = b.Id,
                    Email = b.Email,
                    Name = b.Name,
                    Telp = b.Telp,
                    NIP = b.NIP,
                    NIK = b.NIK,
                    PDB = b.PDB,
                    //PDB = b.PDB.HasValue ? b.PDB.Value.ToString("dd-MMM-yyyy") : "",
                    PositionId = b.Position == null ? 0 : b.Position.Id,
                    PositionName = b.Position == null ? "" : b.Position.Name,
                    PertaminaId = b.Pertamina == null ? 0 : b.Pertamina.Id,
                    PertaminaName = b.Pertamina == null ? "" : b.Pertamina.Name,
                    Role = b.Role,
                    IsLocked = b.IsLocked ? "Yes" : "No",
                    IsActive = b.IsActive ? "Yes" : "No",
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<UserDTModel> sorted;

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

            return new BaseDTResponseModel<UserDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.User.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<User>> GetAll(CancellationToken cancellationToken)
          => await _db.User.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<User> GetById(int id, CancellationToken cancellationToken)
            => await _db.User
            .Include(b => b.Pertamina)
            .Include(b => b.Position)
            .Include(b => b.Foto)
            .SingleOrDefaultAsync(b => b.Id == id && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"User with id '{id}' not found!");

        public async Task<User> Create(UserModel param, CancellationToken cancellationToken)
        {
            var data = new User();
            var c = await _db.User
            .Include(b => b.Pertamina)
            .Include(b => b.Position)
            .Include(b => b.Foto)
            .SingleOrDefaultAsync(b => (b.Email == param.Email || b.NIK == param.NIK) && b.IsDeleted == false, cancellationToken);

            if (c == null)
            {
                data = new User
                {
                    Pertamina = await _db.Pertamina.SingleOrDefaultAsync(b => b.Id == param.PertaminaId, cancellationToken)
                            ?? throw new DataNotFoundException($"Pertamina with id '{param.PertaminaId}' not found!"),
                    NIK = param.NIK,
                    Email = param.Email,
                    Name = param.Name,
                    Telp = param.Telp,
                    NIP = param.NIP,
                    PDB = param.PDB,
                    Position = param.PositionId == null ? null : await _db.Position.SingleOrDefaultAsync(b => b.Id == param.PositionId, cancellationToken)
                            ?? throw new DataNotFoundException($"Position with id '{param.PositionId}' not found!"),

                    Role = param.Role,
                    IsActive = param.IsActive,
                    IsLocked = param.IsLocked,
                    Password = param.Password,
                    CreatedBy = param.CreatedBy,
                    CreatedAt = DateTime.Now,
                };

                await _db.AddAsync(data, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                data = c;
            }
            return data;
        }

        public async Task<User> Update(int id, UserModel param, CancellationToken cancellationToken)
        {
            User result = await
                _db.User
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"User with id '{id}' not found!");
            result.Pertamina = await _db.Pertamina.SingleOrDefaultAsync(b => b.Id == param.PertaminaId, cancellationToken)
                ?? throw new DataNotFoundException($"Pertamina with id '{param.PertaminaId}' not found!");
            result.NIK = param.NIK;
            result.Email = param.Email;
            result.Name = param.Name;
            result.Telp = param.Telp;
            result.NIP = param.NIP;
            result.PDB = param.PDB;
            result.Position = param.PositionId == null ? null : await _db.Position.SingleOrDefaultAsync(b => b.Id == param.PositionId, cancellationToken)
                ?? throw new DataNotFoundException($"Position with id '{param.PositionId}' not found!");
            result.Role = param.Role;
            result.Password = param.Password == null ? result.Password : param.Password;
            result.IsActive = param.IsActive;
            result.IsLocked = param.IsLocked;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<User> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            User result = await _db
                .User
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"User with id '{id}' not found!");

            result.IsDeleted = true;
            result.IsActive = false;
            result.UpdateBy = updatedBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<User> Login(UserModel param, CancellationToken cancellationToken)
        {
            var pertaminaId = 0;
            var data = new User();
            var result = await _db.User
                .Include(b => b.Foto)
                .Include(b => b.Position)
                .SingleOrDefaultAsync(b =>
                b.Email == param.Email
                && b.IsDeleted == false, cancellationToken);

            if (param.IsIdaman)
            {
                var pertamina = await _db.Pertamina.SingleOrDefaultAsync(b => b.Code == param.PertaminaCode, cancellationToken);
                pertaminaId = pertamina == null ? 0 : pertamina.Id;
            }

            if (result == null && param.IsIdaman)
            {
                param.PDB = "-";
                param.NIK = "-";
                param.Telp = "-";
                param.NIP = "-";
                param.IsActive = true;
                param.Role = RoleConstant.GUEST;
                param.CreatedBy = param.Name;
                var pertamina = await _db.Pertamina.SingleOrDefaultAsync(b => b.Code == param.PertaminaCode, cancellationToken);
                param.PertaminaId = pertaminaId;
                data = await Create(param, cancellationToken);
            }
            else if (result != null && param.IsIdaman)
            {
                data = result;
                result.WrongPss = 0;
                result.IsActive = true;
                result.IsLocked = false;
                _db.Update(result);
                await _db.SaveChangesAsync(cancellationToken);
            }
            else if (result != null && !result.IsActive)
            {
                data.IsActive = false;
            }
            else if (result != null && result.IsLocked)
            {
                data.IsLocked = true;
            }
            else if (result != null && result.Password != param.Password)
            {
                result.WrongPss = result.WrongPss + 1;
                if (result.WrongPss == 3)
                {
                    result.IsLocked = true;
                }
                _db.Update(result);
                await _db.SaveChangesAsync(cancellationToken);

                data.Password = "";
                data.WrongPss = result.WrongPss;
            }

            else if (result != null)
            {
                data = result;
                result.WrongPss = 0;
                result.IsActive = true;
                _db.Update(result);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return data;
        }

        public async Task<User> ForgotPassword(UserModel param, CancellationToken cancellationToken)
        {

            var result = await _db.User.Include(b => b.Position)
                .SingleOrDefaultAsync(b =>
                b.Email == param.Email
                && b.IsDeleted == false, cancellationToken);
            return result;
        }

        public async Task<User> ChangePassword(UserModel param, CancellationToken cancellationToken)
        {
            User result = await
                _db.User
                .SingleOrDefaultAsync(b => b.Id == param.Id, cancellationToken)
                ?? throw new DataNotFoundException($"User with id '{param.Id}' not found!");

            if (result.Password == param.Password)
            {
                result.Password = param.PasswordNew;
                result.UpdateBy = param.UpdateBy;
                result.UpdateDt = DateTime.Now;

                _db.Update(result);
                await _db.SaveChangesAsync(cancellationToken);
                return result;
            }
            else
            {
                return new User();
            }
        }


        public async Task<User> ImageUser(int id, UserModel param, CancellationToken cancellationToken)
        {
            User result = await
                _db.User
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"User with id '{id}' not found!");

            result.NIK = param.NIK;
            result.Telp = param.Telp;
            result.NIP = param.NIP;
            result.PDB = param.PDB;
            result.Position = param.PositionId == null ? null : await _db.Position.SingleOrDefaultAsync(b => b.Id == param.PositionId, cancellationToken)
                ?? throw new DataNotFoundException($"Position with id '{param.PositionId}' not found!");

            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            if (param.FotoData != null)
            {
                result.Foto = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/User/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.FotoData,
                    Flag: "User",
                    autoRename: true,
                    isUpdate: false,
                    remark: "User");
            }


            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

    }
}
