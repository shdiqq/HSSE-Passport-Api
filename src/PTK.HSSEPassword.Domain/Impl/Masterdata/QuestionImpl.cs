using Microsoft.AspNetCore.Http.HttpResults;
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
    public class QuestionImpl : IQuestionService
    {
        private readonly HSSEPassportDbContext _db;
        private protected IUploadService<FileUpload> _uploadService;
        public QuestionImpl(HSSEPassportDbContext db, IUploadService<FileUpload> uploadService = null)
        {
            _db = db;
            _uploadService = uploadService;
        }

        public async Task<BaseDTResponseModel<QuestionDTModel>> DataTablePagination(QuestionDTParamModel param, CancellationToken cancellationToken)
        {
            if (param.PageSize == 0)
                return new BaseDTResponseModel<QuestionDTModel>
                {
                    Data = new List<QuestionDTModel>(),
                    Draw = param.Draw,
                    RecordsFiltered = 0,
                    RecordsTotal = await _db.Question.Where(b => b.IsDeleted == false).CountAsync(cancellationToken)
                };

            IQueryable<QuestionDTModel> raw = _db
                .Question
                .Include(b => b.Element)
                .Where(b => !b.IsDeleted
                && ((param.ElementId == null || param.ElementId == 0) || b.Element.Id == param.ElementId)
                && (string.IsNullOrEmpty(param.Name) || b.Name.Contains(param.Name))
                && ((param.Score == null || param.Score == 0) || b.Score == param.Score)
                )
                .Select(b => new QuestionDTModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    ElementId = b.Element == null ? 0 : b.Element.Id,
                    ElementName = b.Element == null ? "" : b.Element.Name,
                    Score = b.Score,
                    IsActive = b.IsActive ? "Yes" : "No",
                    CreatedDT = b.CreatedAt,
                    ImageData = b.Image == null ? null : new FileUploadModel { Id = b.Image.Id, FileName = b.Image.FileName },
                });

            IEnumerable<QuestionDTModel> sorted;

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

            var sortedList = sorted.ToList();
            for (int i = 0; i < sortedList.Count(); i++)
            {
                sortedList[i].Answer = await _db.Answer
                .Include(b => b.Question)
                .Where(b => b.Question.Id == sortedList[i].Id)
                .Select(b => new AnswerModel
                {
                    QuestionId = b.Question == null ? 0 : b.Question.Id,
                    Id = b.Id,
                    Name = b.Name,
                    IsTrue = b.IsTrue,
                }).ToListAsync();
            }


            return new BaseDTResponseModel<QuestionDTModel>
            {
                Data = sortedList,
                Draw = param.Draw,
                RecordsFiltered = totalCount,
                RecordsTotal = await _db.Question.Where(b => b.IsActive == true && b.IsDeleted == false).CountAsync(cancellationToken)
            };
        }

        public async Task<List<Question>> GetAll(CancellationToken cancellationToken)
          => await _db.Question.Where(b => b.IsActive && b.IsDeleted == false).ToListAsync(cancellationToken);

        public async Task<Question> GetById(int id, CancellationToken cancellationToken)
            => await _db.Question
            .Include(b => b.Element).SingleOrDefaultAsync(b => b.Id == id && b.IsActive && b.IsDeleted == false, cancellationToken)
            ?? throw new DataNotFoundException($"Question with id '{id}' not found!");

        public async Task<Question> Create(QuestionModel param, CancellationToken cancellationToken)
        {
            var data = new Question
            {
                Element = await _db.Element.SingleOrDefaultAsync(b => b.Id == param.ElementId, cancellationToken)
                    ?? throw new DataNotFoundException($"Element with id '{param.ElementId}' not found!"),
                Name = param.Name,
                Score = param.Score ?? 0,
                IsActive = param.IsActive,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            await _db.Question.AddAsync(data, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            if (param.ImageData != null)
            {
                data.Image = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Question/Files/" + data.Id,
                    createBy: param.CreatedBy,
                    trxId: data.Id,
                    file: param.ImageData,
                    Flag: "Question",
                    autoRename: true,
                isUpdate: false,
                    remark: "Question");

                _db.Question.Update(data);
                await _db.SaveChangesAsync(cancellationToken);

            }

            var answers = new List<Answer>();
            foreach (var item in param.Answer)
            {
                answers.Add(new Answer
                {
                    Question = data,
                    Name = item.Name,
                    IsTrue = item.IsTrue ?? false,
                    CreatedBy = param.CreatedBy,
                    CreatedAt = DateTime.Now,
                });
            }

            await _db.Answer.AddRangeAsync(answers, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return data;
        }

        public async Task<Question> Update(int id, QuestionModel param, CancellationToken cancellationToken)
        {
            Question result = await
                _db.Question
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Question with id '{id}' not found!");

            result.Element = await _db.Element.SingleOrDefaultAsync(b => b.Id == param.ElementId, cancellationToken)
                    ?? throw new DataNotFoundException($"Element with id '{param.ElementId}' not found!");
            result.Name = param.Name;
            result.Score = param.Score ?? 0;
            result.IsActive = param.IsActive;
            result.UpdateBy = param.UpdateBy;
            result.UpdateDt = DateTime.Now;

            if (param.ImageData != null)
            {
                result.Image = await _uploadService.UploadFileWithReturn(
                    path: $"HSSEPassport/Question/Files/" + result.Id,
                    createBy: param.CreatedBy,
                    trxId: result.Id,
                    file: param.ImageData,
                    Flag: "Question",
                    autoRename: true,
                isUpdate: false,
                    remark: "Question");

            }

            _db.Question.Update(result);
            await _db.SaveChangesAsync(cancellationToken);

            foreach (var item in param.Answer)
            {
                Answer answer = await
                _db.Answer
                .SingleOrDefaultAsync(b => b.Id == item.Id, cancellationToken)
                ?? throw new DataNotFoundException($"Answer with id '{id}' not found!");

                answer.Question = result;
                answer.Name = item.Name;
                answer.IsTrue = item.IsTrue ?? false;
                answer.UpdateBy = param.UpdateBy;
                answer.UpdateDt = DateTime.Now;
                _db.Answer.Update(answer);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        public async Task<Question> Delete(int id, string updatedBy, CancellationToken cancellationToken)
        {
            Question result = await _db
                .Question
                .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new DataNotFoundException($"Question with id '{id}' not found!");

            result.IsDeleted = true;
            result.IsActive = false;
            result.UpdateBy = updatedBy;
            result.UpdateDt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }


        public async Task<Question> PostAllWithAnswer(List<QuestionModel> param, CancellationToken cancellationToken)
        {
            var questions = await _db.Question.ToListAsync(cancellationToken);
            var answers = await _db.Answer.ToListAsync(cancellationToken);
            foreach (var a in param)
            {
                var question = questions.Where(b => b.Id == a.Id).SingleOrDefault();
                question.Name = a.Name;
                question.Score = a.Score ?? 0;
                question.UpdateBy = a.UpdateBy;
                question.UpdateDt = DateTime.Now;
                _db.Question.Update(question);

                foreach (var b in a.Answer)
                {
                    var answer = answers.Where(c => c.Id == b.Id).SingleOrDefault();
                    answer.Name = b.Name;
                    answer.IsTrue = b.IsTrue ?? false;
                    answer.UpdateBy = b.UpdateBy;
                    answer.UpdateDt = DateTime.Now;
                    _db.Answer.Update(answer);
                }
            }
            await _db.SaveChangesAsync(cancellationToken);
            return new Question();
        }


        public async Task<QuestionModel> GetQuestionDetailById(int id, CancellationToken cancellationToken)
        {
            var result = await _db.Question
            .Include(b => b.Element)
            .Where(b => b.Id == id && !b.IsDeleted)
            .Select(b => new QuestionModel
            {
                Id = b.Id,
                Name = b.Name,
                Score = b.Score,
                ElementId = b.Element == null ? 0 : b.Element.Id,
                ElementName = b.Element == null ? "-" : b.Element.Name,
                IsActive = b.IsActive,
            }).SingleOrDefaultAsync(cancellationToken)
            ?? throw new DataNotFoundException($"Question with id '{id}' not found!");
            result.Answer = await _db.Answer
                .Include(b => b.Question)
                .Where(b => b.Question.Id == id)
                .Select(b => new AnswerModel
                {
                    QuestionId = b.Question == null ? 0 : b.Question.Id,
                    Id = b.Id,
                    Name = b.Name,
                    IsTrue = b.IsTrue,
                }).ToListAsync();
            return result;
        }
    }
}
