using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace Praise.Domain.Impl.Praise.MasterData
{
    public class AnswerImpl : IAnswerService
    {
        private readonly HSSEPassportDbContext _db;
        public AnswerImpl(HSSEPassportDbContext db)
        {
            _db = db;
        }

        public async Task<BaseDTResponseModel<AnswerDTModel>> DataTablePagination(AnswerDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<AnswerDTModel>
                {
                    Data = new List<AnswerDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Answer.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<AnswerDTModel> raw = _db
                .Answer
                .Include(b => b.Question)
                .Where(b => (b.IsActive == true && b.IsDeleted == false) &&
                (string.IsNullOrEmpty(param.Name) || b.Name.Contains(param.Name)))
                .Select(b => new AnswerDTModel
                {
                    Id = b.Id,
                    QuestionId = b.Question.Id,
                    QuestionName = b.Question.Name,
                    Name = b.Name,
                    IsTrue = b.IsTrue,
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<AnswerDTModel> sorted;

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

            return new BaseDTResponseModel<AnswerDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Answer.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Answer>> GetAll(CancellationToken cancellationToken)
          => await _db.Answer.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Answer> GetById(int id, CancellationToken cancellationToken)
            => await _db.Answer.SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Answer with id '{id}' not found!");

        public async Task<Answer> Create(AnswerModel param, CancellationToken cancellationToken)
        {
            var data = new Answer
            {
                Question = await
                _db.Question
                .SingleOrDefaultAsync(b => b.Id == param.QuestionId, cancellationToken)
                ?? throw new DataNotFoundException($"Question with id '{param.QuestionId}' not found!"),
                Name = param.Name,
                IsTrue = param.IsTrue ?? false,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<Answer> Update(int id, AnswerModel param, CancellationToken cancellationToken)
        {
            Answer result = await
                _db.Answer
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Question with id '{id}' not found!");

            result.Question = await
                _db.Question
                .SingleOrDefaultAsync(b => b.Id == param.QuestionId, cancellationToken)
                ?? throw new DataNotFoundException($"Question with id '{param.QuestionId}' not found!");
            result.Name = param.Name;
            result.IsTrue = param.IsTrue ?? false;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<Answer> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Answer result = await _db
                .Answer
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Answer with id '{id}' not found!");

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
