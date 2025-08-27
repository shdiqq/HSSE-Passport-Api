using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace Praise.Domain.Impl.Praise.MasterData
{
    public class ElementImpl : IElementService
    {
        private readonly HSSEPassportDbContext _db;
        public ElementImpl(HSSEPassportDbContext db)
        {
            _db = db;
        }

        public async Task<BaseDTResponseModel<ElementDTModel>> DataTablePagination(ElementDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<ElementDTModel>
                {
                    Data = new List<ElementDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Element.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<ElementDTModel> raw = _db
                .Element
                .Where(b => (b.IsActive == true && b.IsDeleted == false) &&
                (string.IsNullOrEmpty(param.Name) || b.Name.Contains(param.Name)))
                .Select(b => new ElementDTModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<ElementDTModel> sorted;

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

            return new BaseDTResponseModel<ElementDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Element.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Element>> GetAll(CancellationToken cancellationToken)
          => await _db.Element.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Element> GetById(int id, CancellationToken cancellationToken)
            => await _db.Element.SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Element with id '{id}' not found!");

        public async Task<Element> Create(ElementModel param, CancellationToken cancellationToken)
        {
            var data = new Element
            {
                Name = param.Name,

                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<Element> Update(int id, ElementModel param, CancellationToken cancellationToken)
        {
            Element result = await
                _db.Element
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Element with id '{id}' not found!");

            result.Name = param.Name;

            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<Element> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Element result = await _db
                .Element
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Element with id '{id}' not found!");

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
