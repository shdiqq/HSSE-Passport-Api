using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Interfaces.Transaction;
using PTK.HSSEPassport.Api.Domain.Models;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Utilities.Base;
using System;

namespace Praise.Domain.Impl.Praise.Transaction
{
    public class TestDetailImpl : ITestDetailService
    {
        private readonly HSSEPassportDbContext _db;
        public TestDetailImpl(HSSEPassportDbContext db)
        {
            _db = db;
        }

        public async Task<BaseDTResponseModel<TestDetailDTModel>> DataTablePagination(TestDetailDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<TestDetailDTModel>
                {
                    Data = new List<TestDetailDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.TestDetail.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<TestDetailDTModel> raw = _db
                .TestDetail
                .Where(b => (b.IsActive == true && b.IsDeleted == false) &&
                (string.IsNullOrEmpty(param.TestName) || b.Question.Name.Contains(param.TestName)))
                .Select(b => new TestDetailDTModel
                {
                    Id = b.Id,
                    //Name = b.Name,
                    CreatedDT = b.CreatedAt
                });

            IEnumerable<TestDetailDTModel> sorted;

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

            return new BaseDTResponseModel<TestDetailDTModel>
            {
                Data = sorted,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.TestDetail.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<TestDetail>> GetAll(CancellationToken cancellationToken)
          => await _db.TestDetail.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<TestDetail> GetById(int id, CancellationToken cancellationToken)
            => await _db.TestDetail.SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"TestDetail with id '{id}' not found!");

        public async Task<TestDetail> Create(TestDetailModel param, CancellationToken cancellationToken)
        {
            var data = new TestDetail
            {
                //Name = param.Name,

                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<TestDetail> Update(int id, TestDetailModel param, CancellationToken cancellationToken)
        {
            TestDetail result = await
                _db.TestDetail
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"TestDetail with id '{id}' not found!");

            //result.Name = param.Name;

            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<TestDetail> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            TestDetail result = await _db
                .TestDetail
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"TestDetail with id '{id}' not found!");

            result.IsDeleted = true;
            result.IsActive = false;
            result.UpdateBy = updatedBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }


        public async Task<List<TestDetail>> GetAllByTestId(int id, CancellationToken cancellationToken)
            => await _db.TestDetail
            .Include(b => b.Test)
            .Include(b => b.Answer)
            .Where(b => b.IsActive && b.IsDeleted == false && b.Test.Id == id).ToListAsync(cancellationToken);



        public async Task<List<ElementModel>> GetQuestionRandom(CancellationToken cancellationToken)
        {
            var data = await _db.Answer
                .Include(b => b.Question)
                .ThenInclude(q => q.Element)
                .Include(b => b.Question.Image)
                .Where(b => b.Question.IsActive && !b.Question.IsDeleted)
                .ToListAsync(cancellationToken);

            // Ambil 20 pertanyaan secara acak terlebih dahulu
            var selectedQuestions = data
                .GroupBy(a => a.Question) // Kelompokkan berdasarkan pertanyaan
                .OrderBy(_ => Guid.NewGuid()) // Acak pertanyaan
                .Take(20) // Ambil maksimal 20 pertanyaan
                .Select(qGroup => new
                {
                    Question = qGroup.Key,
                    Element = qGroup.Key.Element, // Simpan referensi elemen
                    Answers = qGroup.OrderBy(_ => Guid.NewGuid()).ToList() // Acak jawaban
                })
                .ToList();

            // Kelompokkan ulang berdasarkan elemen
            var groupedByElement = selectedQuestions
                .GroupBy(q => q.Element) // Kelompokkan berdasarkan elemen
                .Select(elementGroup => new
                {
                    Element = elementGroup.Key,
                    Questions = elementGroup
                        .Select(q => new
                        {
                            q.Question,
                            q.Answers
                        })
                        .ToList()
                })
                .ToList();

            // Mapping hasil akhir ke tipe data List<Element<List<Question<List<Answer>>>>>
            var result = groupedByElement
                .Select(e => new ElementModel
                {
                    Id = e.Element.Id, // Asumsi ada property Id di Element
                    Name = e.Element.Name, // Sesuaikan dengan properti Element
                    Question = e.Questions.Select(q => new QuestionModel
                    {
                        Id = q.Question.Id, // Asumsi ada property Id di Question
                        Name = q.Question.Name, // Sesuaikan dengan properti Question
                        Score = q.Question.Score,
                        Image = q.Question.Image?.Id == null ? null : new FileUploadModel { Id = q.Question.Image.Id, FileName = q.Question.Image.FileName },
                        Answer = q.Answers.Select(a => new AnswerModel
                        {
                            Id = a.Id, // Asumsi ada property Id di Answer
                            Name = a.Name, // Sesuaikan dengan properti Answer
                        }).ToList()
                    }).ToList()
                }).ToList();

            return result;

        }


        public async Task<List<ElementModel>> GetQuestionAnswer(int id, int passportId, CancellationToken cancellationToken)
        {
            // Ambil data dari database dengan Include dan filter
            var data = await _db.TestDetail
                .Include(b => b.Test)
                .Include(b => b.Test.Passport)
                .Include(b => b.Question)
                .Include(b => b.Question.Element)
                .Include(b => b.Answer)
                .Where(b => b.Test.Id == id && b.Test.Passport.Id == passportId)
                .OrderBy(b => b.Question.Id) // Urutkan berdasarkan Id Question
                .ThenBy(b => b.Answer.Id)    // Urutkan berdasarkan Id Answer
                .ToListAsync(cancellationToken);

            // Kelompokkan data berdasarkan Element
            var groupedByElement = data
                .GroupBy(td => td.Question.Element) // Group berdasarkan Element
                .Select(group => new
                {
                    Element = group.Key,
                    Questions = group
                        .GroupBy(td => td.Question) // Group berdasarkan Question
                        .OrderBy(qGroup => qGroup.Key.Id) // Pastikan pertanyaan tidak diacak
                        .Select(qGroup => new
                        {
                            Question = qGroup.Key,
                            TestDetails = qGroup
                                .OrderBy(td => td.Answer.Id) // Pastikan jawaban tidak diacak
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            // Mapping hasil akhir ke tipe data List<Element<List<Question<List<TestDetail>>>>>
            var result = groupedByElement
                .Select(e => new ElementModel
                {
                    Id = e.Element.Id, // Properti Id di Element
                    Name = e.Element.Name, // Properti Name di Element
                    Question = e.Questions.Select(q => new QuestionModel
                    {
                        Id = q.Question.Id, // Properti Id di Question
                        Name = q.Question.Name, // Properti Name di Question
                        Score = q.Question.Score,
                        TestDetail = q.TestDetails.Select(a => new TestDetailModel
                        {
                            Id = a.Id, // Properti Id di TestDetail
                            AnswerName = a.Answer.Name, // Properti Name di Answer
                            IsTrue = a.IsTrue,
                            Score = a.Score,
                            Total = a.Total,
                        }).ToList() // Jawaban sudah dalam urutan aslinya
                    }).ToList() // Pertanyaan sudah dalam urutan aslinya
                }).ToList();

            return result;
        }

    }
}
