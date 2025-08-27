using Microsoft.AspNetCore.Mvc;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using PTK.HSSEPassport.Api.Domain.Interfaces.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Api.Utilities.Base;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Base;
using System.Threading;

namespace PTK.HSSEPassport.Api.Controllers.Transaction
{
    [ApiController]
    [Route("api/v1/Transaction/[controller]")]
    [Produces("application/json")]
    public class PassportController : BaseController
    {
        private protected IPassportService _PassportRepo;

        public PassportController(IAppLogService appLog, IPassportService PassportRepo) : base(appLog)
        {
            _PassportRepo = PassportRepo;
        }

        [HttpPost("DataTable")]
        public async Task<IActionResult> DataTablePagination(PassportDTParamModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.DataTablePagination(param, cancellationToken);
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
                var result = await _PassportRepo.GetById(id, cancellationToken);
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
                var result = await _PassportRepo.GetAll(cancellationToken);
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
        public async Task<IActionResult> Create(PassportModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.Create(param, cancellationToken);
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
        public async Task<IActionResult> Update([FromRoute] int id, PassportModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.Update(id, param, cancellationToken);
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
                var result = await _PassportRepo.Delete(id, updatedBy, cancellationToken);
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


        [HttpPut("Regulation/{id:int}")]
        public async Task<IActionResult> Regulation([FromRoute] int id, [FromBody] PassportModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.Regulation(id, param, cancellationToken);

                // update regulation
                await _PassportRepo.UpdateRegulation(id, cancellationToken);
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

        [HttpPost("ChangeStatus/{id:int}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] PassportModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.ChangeStatus(id, param, cancellationToken);
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


        [HttpGet("DashboardStatus")]
        public async Task<IActionResult> DashboardStatus([FromQuery] ParamDashboardModel vm, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.DashboardStatus(vm.Year, vm.CreatedBy, cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpGet("DashboardExpired")]
        public async Task<IActionResult> DashboardExpired([FromQuery] ParamDashboardModel vm, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.DashboardExpired(vm.Year, vm.CreatedBy, cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }

        [HttpGet("DashboardRequestExpired")]
        public async Task<IActionResult> DashboardRequestExpired([FromQuery] ParamDashboardModel vm, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _PassportRepo.DashboardRequestExpired(vm.Year, vm.CreatedBy, cancellationToken);
                return Ok(new ReturnJson { Payload = result });
            }
            catch (DataNotFoundException e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), vm.Year.ToString(), GeneralConstant.FAILED, cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                throw;
            }
        }
    }
}
