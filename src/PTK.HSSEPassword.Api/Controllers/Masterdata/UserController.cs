using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Api.Utilities.Base;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Data.Service.Session;
using PTK.HSSEPassport.Utilities.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PTK.HSSEPassport.Api.Controllers.Masterdata
{
    [ApiController]
    [Route("api/v1/Masterdata/[controller]")]
    [Produces("application/json")]
    public class UserController : BaseController
    {
        private protected IUserService _UserRepo;
        //private readonly IWhatsAppClientService _whatsAppClient;
        public UserController(IAppLogService appLog, IUserService UserRepo) : base(appLog)
        {
            _UserRepo = UserRepo;
            //_whatsAppClient = whatsAppClient;
        }

        [Authorize]
        [HttpGet("AuthStatus")]
        public IActionResult AuthStatus() => Ok(new { ok = true, utc = DateTime.UtcNow });

        [HttpPost("DataTable")]
        public async Task<IActionResult> DataTablePagination(UserDTParamModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _UserRepo.DataTablePagination(param, cancellationToken);
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
                var result = await _UserRepo.GetById(id, cancellationToken);
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
                var result = await _UserRepo.GetAll(cancellationToken);
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
        public async Task<IActionResult> Create([FromBody] UserModel param, CancellationToken cancellationToken)
        {
            try
            {
                if (param.Password != null)
                {
                    param.Password = EncryptPassword(param.Password);
                }
                var result = await _UserRepo.Create(param, cancellationToken);
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserModel param, CancellationToken cancellationToken)
        {
            try
            {
                if (param.Password != null)
                {
                    param.Password = EncryptPassword(param.Password);
                }
                var result = await _UserRepo.Update(id, param, cancellationToken);
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
                var result = await _UserRepo.Delete(id, updatedBy, cancellationToken);
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


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserModel param, CancellationToken cancellationToken, [FromServices] ISessionStoreService sessionStore, [FromServices] IConfiguration cfg)
        {
            try
            {
                param.Password = EncryptPassword(param.Password ?? GeneralConstant.KODE_DEFAULT);
                var result = await _UserRepo.Login(param, cancellationToken);
                if (result == null)
                {
                    return StatusCode(500, new { ErrorMsg = "Email not found!", IsSuccess = false });
                }
                else if (result.IsLocked)
                {
                    return StatusCode(500, new { ErrorMsg = "Account Locked!", IsSuccess = false });
                }
                else if (!result.IsActive)
                {
                    return StatusCode(500, new { ErrorMsg = "Account not active!", IsSuccess = false });
                }
                else if (result.Password == "")
                {
                    return StatusCode(500, new { ErrorMsg = result.WrongPss + " Wrong password!", IsSuccess = false });
                }

                // ======== SUKSES: daftar sesi & terbitkan JWT ========
                int userId = result.Id; // pastikan tipe int
                string sessionId = Guid.NewGuid().ToString("N");
                string userAgent = Request.Headers["User-Agent"].ToString();

                // simpan sesi aktif (tanpa kadaluarsa server-side)
                await sessionStore.SetCurrentSessionAsync(userId, sessionId, expiresAt: null, deviceInfo: userAgent);

                // build JWT
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), new Claim(JwtRegisteredClaimNames.Jti, sessionId), new Claim(ClaimTypes.Name, result.Name ?? result.Email ?? result.NIK ?? "User") };

                var token = new JwtSecurityToken(
                    issuer: cfg["Jwt:Issuer"],
                    audience: null,
                    claims: claims,
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                var payload = result;

                return Ok(new ReturnJson { Payload = payload, Token = jwt, TokenType = "Bearer" });
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

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromServices] ISessionStoreService sessionStore, [FromQuery] bool all = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                       ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
                var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);

                if (int.TryParse(sub, out var userId))
                {
                    if (all)
                    {
                        await sessionStore.RevokeCurrentAsync(userId);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(jti))
                            await sessionStore.RevokeSessionAsync(userId, jti);
                    }
                }

                return Ok(new ReturnJson { Payload = new { Message = "Logged out" } });
            }
            catch (Exception e)
            {
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED,
                    cancellationToken, errorMessage: e.InnerException?.Message ?? e.Message, info: "API");
                return StatusCode(500, new { ErrorMsg = "Logout failed", IsSuccess = false });
            }
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _UserRepo.ForgotPassword(param, cancellationToken);
                //if (result != null)
                //{
                //    await _whatsAppClient.Send(new WhatsAppClientSendRequestModel
                //    {
                //        Application = "HSSEPassport",
                //        IsSkipProcess = true,
                //        Module = "Lupa Password",
                //        PhoneNumber = DecryptPassword(result.Telp),
                //        Text = $"Email {result.Email}, Password anda adalah {DecryptPassword(result.Password)}, mohon dapat menyimpan password tersebut dengan baik. Terima kasih",
                //    }, cancellationToken);
                //}
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

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserModel param, CancellationToken cancellationToken)
        {
            try
            {
                param.Password = EncryptPassword(param.Password);
                param.PasswordNew = EncryptPassword(param.PasswordNew);
                var result = await _UserRepo.ChangePassword(param, cancellationToken);
                if (result.Id > 0)
                {
                    return Ok(new ReturnJson { Payload = result });
                }
                await SaveAppLog(GetCurrentMethod(), GeneralConstant.NO_TRX_ID, GeneralConstant.FAILED, cancellationToken, errorMessage: "Old password does not match!", info: "API");
                return StatusCode(500, new { ErrorMsg = "Old password does not match!", IsSuccess = false });
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

        [HttpPut("ImageUser/{id:int}")]
        public async Task<IActionResult> ImageUser([FromRoute] int id, UserModel param, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _UserRepo.ImageUser(id, param, cancellationToken);
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
