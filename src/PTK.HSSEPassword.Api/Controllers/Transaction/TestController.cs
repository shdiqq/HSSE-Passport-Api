using Microsoft.AspNetCore.Mvc;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using PTK.HSSEPassport.Api.Domain.Interfaces.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Api.Utilities.Base;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Controllers.Transaction
{
    [ApiController]
    [Route("api/v1/Transaction/[controller]")]
    [Produces("application/json")]
    public class TestController : BaseController
    {
        private protected ITestService _TestRepo;
        private protected ITestDetailService _TestDetailRepo;
        private protected IPassportService _PassportRepo;

        public TestController(IAppLogService appLog, ITestService TestRepo, IPassportService passportRepo = null, ITestDetailService testDetailRepo = null) : base(appLog)
        {
            _TestRepo = TestRepo;
            _PassportRepo = passportRepo;
            _TestDetailRepo = testDetailRepo;
        }

        [HttpPost("DataTable")]
        public async Task<IActionResult> DataTablePagination(TestDTParamModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _TestRepo.DataTablePagination(param, cancellationToken);
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
                var result = await _TestRepo.GetById(id, cancellationToken);
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
                var result = await _TestRepo.GetAll(cancellationToken);
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
        public async Task<IActionResult> Create(TestModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _TestRepo.Create(param, cancellationToken);

                // update regulation
                await _PassportRepo.UpdateRegulation(result.Passport.Id, cancellationToken);

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
        public async Task<IActionResult> Update([FromRoute] int id, TestModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _TestRepo.Update(id, param, cancellationToken);
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
                var result = await _TestRepo.Delete(id, updatedBy, cancellationToken);
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

        [HttpGet("Detail/{id:int}")]
        public async Task<IActionResult> Detail([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _TestDetailRepo.GetAllByTestId(id, cancellationToken);
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


        [HttpGet("GetQuestionRandom")]
        public async Task<IActionResult> GetQuestionRandom(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _TestDetailRepo.GetQuestionRandom(cancellationToken);
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

        [HttpGet("GetQuestionAnswer/{id:int}/{passportId}")]
        public async Task<IActionResult> GetQuestionAnswer(int id, int passportId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _TestDetailRepo.GetQuestionAnswer(id, passportId, cancellationToken);
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
