using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Api.Domain.Models;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Base;
using PTK.HSSEPassport.Data.Service.Upload;

namespace Praise.Domain.Impl.Praise.Transaction
{
    public class PassportImpl : IPassportService
    {
        private readonly HSSEPassportDbContext _db;
        private protected IUploadService<FileUpload> _uploadService;
        public PassportImpl(HSSEPassportDbContext db, IUploadService<FileUpload> uploadService)
        {
            _db = db;
            _uploadService = uploadService;
        }

        public async Task<BaseDTResponseModel<PassportDTModel>> DataTablePagination(PassportDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<PassportDTModel>
                {
                    Data = new List<PassportDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Passport.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<PassportDTModel> raw = _db
                .Passport
                .Include(b => b.User)
                .Include(b => b.FileSKCK)
                .Include(b => b.FileMCU)
                .Include(b => b.FileSBSS)
                .Where(b => (b.IsActive == true && b.IsDeleted == false)
                && (param.Year == null || b.Date.Year == param.Year)
                && (string.IsNullOrEmpty(param.Email) || b.User.Email.Contains(param.Email))
                && (string.IsNullOrEmpty(param.PertaminaId) || b.User.Pertamina.Id == int.Parse(param.PertaminaId))
                && (string.IsNullOrEmpty(param.Status) || b.Status == param.Status)
                && (string.IsNullOrEmpty(param.CreatedBy) || b.CreatedBy == param.CreatedBy))
                .Select(b => new PassportDTModel
                {
                    Id = b.Id,
                    PertaminaName = b.User.Pertamina == null ? "" : b.User.Pertamina.Name,
                    UserId = b.User.Id,
                    Email = b.User.Email,
                    UserName = b.User.Name,
                    NIP = b.User.NIP,
                    NIK = b.User.NIK,
                    Date = b.Date.ToString("dd-MMM-yyyy"),
                    Expired = b.Expired == null ? "" : b.Expired.Value.ToString("dd-MMM-yyyy"),
                    FileSKCKData = b.FileSKCK == null ? null : new FileUploadModel { Id = b.FileSKCK.Id, FileName = b.FileSKCK.FileName },
                    FileMCUData = b.FileMCU == null ? null : new FileUploadModel { Id = b.FileMCU.Id, FileName = b.FileMCU.FileName },
                    FileSBSSData = b.FileSBSS == null ? null : new FileUploadModel { Id = b.FileSBSS.Id, FileName = b.FileSBSS.FileName },
                    IsDemoRoom = b.IsDemoRoom,
                    DemoRoomDate = b.DemoRoomDate == null ? "" : b.DemoRoomDate.Value.ToString("dd-MMM-yyyy"),
                    PreTest = b.PreTest,
                    PostTest = b.PostTest,
                    Fraud = b.Fraud,
                    Status = b.Status,
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<PassportDTModel> sorted;

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

            return new BaseDTResponseModel<PassportDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Passport.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Passport>> GetAll(CancellationToken cancellationToken)
          => await _db.Passport.Include(b => b.User).Where(b => b.Status == RoleStatus.DONE && b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Passport> GetById(int id, CancellationToken cancellationToken)
            => await _db.Passport
            .Include(b => b.User)
            .Include(b => b.User.Foto)
            .Include(b => b.User.Position)
            .Include(b => b.User.Pertamina)
            .Include(b => b.User.Pertamina.Logo)
            .Include(b => b.User.Pertamina.OfficialSign)
            .Include(b => b.FileMCU)
            .Include(b => b.FileSKCK)
            .Include(b => b.FileSBSS)
            .SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Passport with id '{id}' not found!");

        public async Task<Passport> Create(PassportModel param, CancellationToken cancellationToken)
        {
            var data = new Passport
            {
                User = await _db.User.SingleOrDefaultAsync(b => b.Id == param.UserId, cancellationToken)
                    ?? throw new DataNotFoundException($"User with id '{param.UserId}' not found!"),

                Date = param.Date ?? DateOnly.MinValue,
                Expired = param.Expired,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
                Status = RoleStatus.REQUEST
            };

            await _db.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);


            if (param.FileSKCKData != null)
            {
                data.FileSKCK = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Passport/Files/" + data.Id,
                    createBy: param.CreatedBy,
                    trxId: data.Id,
                    file: param.FileSKCKData,
                    Flag: "Passport",
                    autoRename: true,
                    isUpdate: false,
                    remark: "SKCK");

                _db.Passport.Update(data);
                await _db.SaveChangesAsync(cancellationToken);
            }

            if (param.FileMCUData != null)
            {
                data.FileMCU = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Passport/Files/" + data.Id,
                    createBy: param.CreatedBy,
                    trxId: data.Id,
                    file: param.FileMCUData,
                    Flag: "Passport",
                    autoRename: true,
                    isUpdate: false,
                    remark: "MCU");

                _db.Passport.Update(data);
                await _db.SaveChangesAsync(cancellationToken);
            }

            if (param.FileSBSSData != null)
            {
                data.FileSBSS = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Passport/Files/" + data.Id,
                    createBy: param.CreatedBy,
                    trxId: data.Id,
                    file: param.FileSBSSData,
                    Flag: "Passport",
                    autoRename: true,
                    isUpdate: false,
                    remark: "SBSS");

                _db.Passport.Update(data);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return data;
        }

        public async Task<Passport> Update(int id, PassportModel param, CancellationToken cancellationToken)
        {
            Passport result = await
                _db.Passport
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Passport with id '{id}' not found!");

            result.User = await _db.User.SingleOrDefaultAsync(b => b.Id == param.UserId, cancellationToken)
                   ?? throw new DataNotFoundException($"User with id '{param.UserId}' not found!");

            result.Date = param.Date ?? DateOnly.MinValue;
            result.Expired = param.Expired;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;



            if (param.FileSKCKData != null)
            {
                result.FileSKCK = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Passport/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.FileSKCKData,
                    Flag: "Passport",
                    autoRename: true,
                    isUpdate: false,
                    remark: "SKCK");

            }

            if (param.FileMCUData != null)
            {
                result.FileMCU = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Passport/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.FileMCUData,
                    Flag: "Passport",
                    autoRename: true,
                    isUpdate: false,
                    remark: "MCU");
            }

            if (param.FileSBSSData != null)
            {
                result.FileSBSS = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Passport/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.FileSBSSData,
                    Flag: "Passport",
                    autoRename: true,
                    isUpdate: false,
                    remark: "SBSS");
            }


            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<Passport> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Passport result = await _db
                .Passport
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Passport with id '{id}' not found!");

            result.IsDeleted = true;
            result.IsActive = false;
            result.UpdateBy = updatedBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }


        public async Task<Passport> Regulation(int id, PassportModel param, CancellationToken cancellationToken)
        {
            Passport result = await
                _db.Passport
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Passport with id '{id}' not found!");

            result.DemoRoomDate = param.DemoRoomDate == null ? result.DemoRoomDate : param.DemoRoomDate;
            result.IsDemoRoom = param.IsDemoRoom ?? result.IsDemoRoom;
            result.PreTest = param.PreTest ?? result.PreTest;
            result.PostTest = param.PostTest ?? result.PostTest;
            result.Status = param.Status ?? result.Status;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }


        public async Task<Passport> UpdateRegulation(int id, CancellationToken cancellationToken)
        {
            Passport result = await
                _db.Passport
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Passport with id '{id}' not found!");



            var test = await _db.Test.Where(b => b.Passport.Id == id).ToListAsync();
            var preTest = test.Where(b => b.Flag == GeneralConstant.PreTest);
            var postTest = test.Where(b => b.Flag == GeneralConstant.PostTest);

            result.PreTest = preTest.Count() > 0 ? preTest.LastOrDefault().Score : 0;
            result.PostTest = postTest.Count() > 0 ? postTest.LastOrDefault().Score : 0;
            result.Fraud = await _db.Fraud.Where(b => b.Passport.Id == id).CountAsync();
            result.UpdateDt = DateTime.Now;


            // tambahan jika dibawah 70 lebih dari 3 kali posttest maka rubah status jadi Request
            if (result.Status == RoleStatus.DEMOROOM
                && postTest.Count() > 0
                && postTest.Count() % 3 == 0
                && result.PostTest < 70)
            {
                result.Status = RoleStatus.REQUEST;
            }



            // untuk update status secara otomatis
            if (result.Status == RoleStatus.DEMOROOM && result.PostTest >= 70)
            {
                result.Status = RoleStatus.APPROVE;

                // jika sudah selesai atau approve otomatis
                if (result.Expired == null)
                {
                    result.Expired = DateOnly.FromDateTime(DateTime.Now).AddYears(6);// berlaku 2 tahun setelah posttest
                }
            }

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<Passport> ChangeStatus(int id, PassportModel param, CancellationToken cancellationToken)
        {
            Passport result = await _db
                .Passport
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Passport with id '{id}' not found!");

            result.Status = param.Status;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<ChartDataModel> DashboardStatus(int year, string createdBy, CancellationToken cancellationToken)
        {
            // Ambil data dari database (contoh: Passport)
            var chartData = new ChartDataModel
            {
                Name = "Percentage",
                ColorByPoint = true,
                Data = _db.Passport
                    .Where(p => p.Date.Year == year && (string.IsNullOrEmpty(createdBy) || p.CreatedBy == createdBy))
                    .GroupBy(p => p.Status) // Kelompokkan data berdasarkan "Status"
                    .Select(g => new ChartDataItem
                    {
                        Name = g.Key, // Gunakan "Status" sebagai nama
                        Y = g.Count() // Hitung jumlah item di setiap status
                    })
                    .ToList()
            };

            //// Contoh menambahkan properti "Sliced" dan "Selected" secara manual
            //if (chartData.Data.Any())
            //{
            //    chartData.Data[1].Sliced = true; // Demo Room
            //    chartData.Data[1].Selected = true;
            //}

            return chartData; // Return dalam format JSON
        }

        public async Task<ChartDataModel> DashboardExpired(int year, string createdBy, CancellationToken cancellationToken)
        {

            var today = DateOnly.FromDateTime(DateTime.Today);
            // Ambil data dari database (contoh: Passport)
            var chartData = new ChartDataModel
            {
                Name = "Percentage",
                ColorByPoint = true,
                Data = _db.Passport
                    .Where(p => (p.Date.Year == year || p.Expired.HasValue && p.Expired.Value.Year == year) && (string.IsNullOrEmpty(createdBy) || p.CreatedBy == createdBy))
                    .Select(p => new
                    {
                        Status = p.Expired.HasValue && p.Expired.Value < today ? "Expired" : "Active"
                    })
                    .GroupBy(p => p.Status) // Kelompokkan berdasarkan status (Expired/Active)
                    .Select(g => new ChartDataItem
                    {
                        Name = g.Key,   // "Expired" atau "Active"
                        Y = g.Count()   // Jumlah item di setiap status
                    })
                    .ToList()
            };

            return chartData; // Return dalam format JSON
        }
        public async Task<List<SeriesDataModel>> DashboardRequestExpired(int year, string createdBy, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Today); // Konversi DateTime ke DateOnly

            // Inisialisasi data kosong dengan 12 bulan (nilai default 0)
            var requestData = Enumerable.Repeat(0, 12).ToList();
            var expiredData = Enumerable.Repeat(0, 12).ToList();

            // Ambil data dari database
            var requestQuery = _db.Passport
                .Where(p => p.Date.Year == year && (string.IsNullOrEmpty(createdBy) || p.CreatedBy == createdBy))
                .GroupBy(p => p.Date.Month)
                .Select(g => new { Month = g.Key, Total = g.Count() })
                .ToList();

            var expiredQuery = _db.Passport
            .Where(p => p.Expired.HasValue && p.Expired <= today && (string.IsNullOrEmpty(createdBy) || p.CreatedBy == createdBy)) // Periksa apakah Expired tidak null
            .GroupBy(p => p.Expired.Value.Month) // Gunakan Value karena sudah dipastikan tidak null
            .Select(g => new
            {
                Month = g.Key,
                Total = g.Count()
            })
            .ToList();

            // Isi data per bulan (replace nilai default 0)
            foreach (var item in requestQuery)
            {
                requestData[item.Month - 1] = item.Total; // Index bulan dimulai dari 0
            }

            foreach (var item in expiredQuery)
            {
                expiredData[item.Month - 1] = item.Total; // Index bulan dimulai dari 0
            }

            // Buat format data JSON
            var seriesData = new List<SeriesDataModel>
            {
                    new SeriesDataModel{ Name= "Request" , Data =requestData},
                    new SeriesDataModel{ Name= "Expired" , Data =expiredData},
            };

            // Return dalam format JSON
            return seriesData;
        }
    }

}

