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
	public class FraudController : BaseController
	{
		private protected IFraudService _FraudRepo;
		private protected IPassportService _PassportRepo;

        public FraudController(IAppLogService appLog, IFraudService FraudRepo, IPassportService passportRepo) : base(appLog)
        {
            _FraudRepo = FraudRepo;
            _PassportRepo = passportRepo;
        }

        [HttpPost("DataTable")]
		public async Task<IActionResult> DataTablePagination(FraudDTParamModel param, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _FraudRepo.DataTablePagination(param, cancellationToken);
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
				var result = await _FraudRepo.GetById(id, cancellationToken);
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
				var result = await _FraudRepo.GetAll(cancellationToken);
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
		public async Task<IActionResult> Create(FraudModel param, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _FraudRepo.Create(param, cancellationToken);

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
		public async Task<IActionResult> Update([FromRoute] int id, FraudModel param, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _FraudRepo.Update(id, param, cancellationToken);

                // update regulation
                await _PassportRepo.UpdateRegulation(result.Passport.Id, cancellationToken);

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
				var result = await _FraudRepo.Delete(id, updatedBy, cancellationToken);
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
	}
}
