using Microsoft.AspNetCore.Mvc;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class FileController : BaseController
    {
        private protected IFileService _fileService;

        public FileController(IAppLogService appLog, IFileService fileService) : base(appLog)
        {
            _fileService = fileService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFileStream([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fileService.GetFile(id, cancellationToken);
                return File(result.FileContents, result.ContentType);
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

        public class TestModel 
        {
            public IFormFile file { get; set; }
        }

        [HttpPost("TestInput")]
        public async Task<IActionResult> TestInput([FromForm] TestModel p, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fileService.TestInput(p.file, cancellationToken);
                return Ok(result);
            }
            catch (DataNotFoundException e)
            {
                return StatusCode(404, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (DomainLayerException e)
            {
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMsg = e.Message, IsSuccess = false });
            }
        }
    }
}
