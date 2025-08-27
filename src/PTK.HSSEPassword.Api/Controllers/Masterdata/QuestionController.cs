using Microsoft.AspNetCore.Mvc;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Api.Utilities.Base;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Controllers.Masterdata
{
    [ApiController]
    [Route("api/v1/Masterdata/[controller]")]
    [Produces("application/json")]
    public class QuestionController : BaseController
    {
        private protected IQuestionService _QuestionRepo;
        private protected IAnswerService _AnswerRepo;

        public QuestionController(IAppLogService appLog, IQuestionService QuestionRepo, IAnswerService answerRepo) : base(appLog)
        {
            _QuestionRepo = QuestionRepo;
            _AnswerRepo = answerRepo;
        }

        [HttpPost("DataTable")]
        public async Task<IActionResult> DataTablePagination(QuestionDTParamModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _QuestionRepo.DataTablePagination(param, cancellationToken);
                return Ok(result);
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _QuestionRepo.GetById(id, cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _QuestionRepo.GetAll(cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _QuestionRepo.Create(param, cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, QuestionModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _QuestionRepo.Update(id, param, cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromQuery] string updatedBy, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _QuestionRepo.Delete(id, updatedBy, cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), id.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpGet("GetAllWithAnswer")]
        public async Task<IActionResult> GetAllWithAnswer(CancellationToken cancellationToken)
        {
            try
            {
                var r = await _QuestionRepo.GetAll(cancellationToken);
                var s = await _AnswerRepo.GetAll(cancellationToken);
                var result = new List<QuestionModel>();
                foreach (var a in r)
                {
                    result.Add(new QuestionModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Score = a.Score,
                        Answer = s.Where(b => b.Question.Id == a.Id)
                        .Select(b => new AnswerModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            IsTrue = b.IsTrue
                        }).ToList(),
                    });
                }

                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpPost("PostAllWithAnswer")]
        public async Task<IActionResult> PostAllWithAnswer(List<QuestionModel> param, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _QuestionRepo.PostAllWithAnswer(param, cancellationToken);

                var r = await _QuestionRepo.GetAll(cancellationToken);
                var s = await _AnswerRepo.GetAll(cancellationToken);
                var result = new List<QuestionModel>();
                foreach (var a in r)
                {
                    result.Add(new QuestionModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Score = a.Score,
                        Answer = s.Where(b => b.Question.Id == a.Id)
                        .Select(b => new AnswerModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            IsTrue = b.IsTrue
                        }).ToList(),
                    });
                }

                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

		[HttpGet("GetQuestionDetailById")]
		public async Task<IActionResult> GetQuestionDetailById(int id, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _QuestionRepo.GetQuestionDetailById(id, cancellationToken);
				return Ok(new ReturnJson { Payload = result });
			}
			catch (DataNotFoundException e)
			{
				await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
				return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
			}
			catch (DomainLayerException e)
			{
				await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
				return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
			}
			catch (Exception e)
			{
				await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
				throw;
			}
		}

		
	}
}
