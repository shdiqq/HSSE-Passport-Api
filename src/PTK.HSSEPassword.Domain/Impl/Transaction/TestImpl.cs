using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Utilities.Base;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Praise.Domain.Impl.Praise.Transaction
{
    public class TestImpl : ITestService
    {
        private readonly HSSEPassportDbContext _db;
        public TestImpl(HSSEPassportDbContext db)
        {
            _db = db;
        }

        public async Task<BaseDTResponseModel<TestDTModel>> DataTablePagination(TestDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<TestDTModel>
                {
                    Data = new List<TestDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Test.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<TestDTModel> raw = _db
                .Test
                .Include(b => b.Passport)
                .Where(b => (b.IsActive == true && b.IsDeleted == false)
                && (param.PassportId == 0 || b.Passport.Id == param.PassportId)
                && (string.IsNullOrEmpty(param.Flag) || b.Flag.Contains(param.Flag))
                )
                .Select(b => new TestDTModel
                {
                    Id = b.Id,
                    Date = b.Date,
                    Score = b.Score,
                    PassportId = b.Passport.Id,
                    Flag = b.Flag,
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<TestDTModel> sorted;

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

            return new BaseDTResponseModel<TestDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Test.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Test>> GetAll(CancellationToken cancellationToken)
          => await _db.Test.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Test> GetById(int id, CancellationToken cancellationToken)
            => await _db.Test.SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Test with id '{id}' not found!");

        public async Task<Test> Create(TestModel param, CancellationToken cancellationToken)
        {
            var data = new Test
            {
                Passport = await _db.Passport.SingleOrDefaultAsync(b => b.Id == param.PassportId, cancellationToken)
                    ?? throw new DataNotFoundException($"Test with id '{param.PassportId}' not found!"),

                Date = param.Date,
                Score = 0,
                Flag = param.Flag,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.Test.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);


            var questions = await _db.Question.ToListAsync(cancellationToken);
            var answers = await _db.Answer.ToListAsync(cancellationToken);
            var detail = new List<TestDetail>();

            var total = 0;
            foreach (var item in param.Question)
            {
                var quest = questions.Where(b => b.Id == item.Id).SingleOrDefault();
                foreach (var a in item.Answer)
                {
                    var answ = answers.Where(b => b.Id == a.Id).SingleOrDefault();

                    int score = 0;
                    bool cek = false;
                    if (answ.IsTrue == true && answ.IsTrue == a.IsTrue)
                    {
                        score = quest.Score;
                        cek = true;
                    }
                    detail.Add(new TestDetail
                    {
                        Test = data,
                        Question = quest,
                        Answer = answ,
                        Score = score,
                        Total = score,
                        IsTrue = a.IsTrue ?? false,
                        CreatedBy = param.CreatedBy,
                        CreatedAt = DateTime.Now,
                    });

                    // untuk menambahkan score
                    total += score;
                }
            }

            await _db.TestDetail.AddRangeAsync(detail, cancellationToken);

            data.Score = total;
            _db.Update(data);

            await _db.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<Test> Update(int id, TestModel param, CancellationToken cancellationToken)
        {
            Test result = await
                _db.Test
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Test with id '{id}' not found!");

            //result.Name = param.Name;

            result.Passport = await _db.Passport.SingleOrDefaultAsync(b => b.Id == param.PassportId, cancellationToken)
                    ?? throw new DataNotFoundException($"Test with id '{param.PassportId}' not found!");

            result.Date = param.Date;
            //result.Score = param.Score;
            result.Flag = param.Flag;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<Test> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Test result = await _db
                .Test
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Test with id '{id}' not found!");

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
