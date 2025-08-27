using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace Praise.Domain.Impl.Praise.MasterData
{
    public class ConfigImpl : IConfigService
    {
        private readonly HSSEPassportDbContext _db;
        public ConfigImpl(HSSEPassportDbContext db)
        {
            _db = db;
        }

        public async Task<BaseDTResponseModel<ConfigDTModel>> DataTablePagination(ConfigDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<ConfigDTModel>
                {
                    Data = new List<ConfigDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Config.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<ConfigDTModel> raw = _db
                .Config
                .Where(b => (b.IsActive == true && b.IsDeleted == false) &&
                (string.IsNullOrEmpty(param.Name) || b.Name.Contains(param.Name)))
                .Select(b => new ConfigDTModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Note = b.Note,
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<ConfigDTModel> sorted;

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

            return new BaseDTResponseModel<ConfigDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Config.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Config>> GetAll(CancellationToken cancellationToken)
          => await _db.Config.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Config> GetById(int id, CancellationToken cancellationToken)
            => await _db.Config.SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Config with id '{id}' not found!");

        public async Task<Config> Create(ConfigModel param, CancellationToken cancellationToken)
        {
            var data = new Config
            {
                Name = param.Name,
                Note = param.Note,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<Config> Update(int id, ConfigModel param, CancellationToken cancellationToken)
        {
            Config result = await
                _db.Config
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Config with id '{id}' not found!");

            result.Name = param.Name;
            result.Note = param.Note;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<Config> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Config result = await _db
                .Config
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Config with id '{id}' not found!");

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
