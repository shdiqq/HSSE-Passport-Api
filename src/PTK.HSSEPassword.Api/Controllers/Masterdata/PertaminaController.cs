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
	public class PertaminaController : BaseController
	{
		private protected IPertaminaService _PertaminaRepo;

		public PertaminaController(IAppLogService appLog, IPertaminaService PertaminaRepo) : base(appLog)
		{
			_PertaminaRepo = PertaminaRepo;
		}

		[HttpPost("DataTable")]
		public async Task<IActionResult> DataTablePagination(PertaminaDTParamModel param, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _PertaminaRepo.DataTablePagination(param, cancellationToken);
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
				var result = await _PertaminaRepo.GetById(id, cancellationToken);
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
				var result = await _PertaminaRepo.GetAll(cancellationToken);
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
		public async Task<IActionResult> Create(PertaminaModel param, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _PertaminaRepo.Create(param, cancellationToken);
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
		public async Task<IActionResult> Update([FromRoute] int id, PertaminaModel param, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _PertaminaRepo.Update(id, param, cancellationToken);
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
				var result = await _PertaminaRepo.Delete(id, updatedBy, cancellationToken);
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
