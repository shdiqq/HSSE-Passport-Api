using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using PTK.HSSEPassport.Data.Service.Session;

namespace PTK.HSSEPassword.Api.Auth
{
  public sealed class SessionAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
  {
    public const string SchemeName = "Session";
    private readonly ISessionStoreService _sessions;

    public SessionAuthenticationHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock,
      ISessionStoreService sessions)
      : base(options, logger, encoder, clock)
    {
      _sessions = sessions;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      if (!Request.Cookies.TryGetValue("sid", out var sid) || string.IsNullOrWhiteSpace(sid))
        return AuthenticateResult.NoResult();

      var rec = await _sessions.FindBySessionAsync(sid);
      if (rec is null) return AuthenticateResult.Fail("SESSION_NOT_FOUND");
      if (!rec.Value.IsActive) return AuthenticateResult.Fail("SESSION_REVOKED");
      if (rec.Value.ExpiresAt is DateTime exp && exp <= DateTime.UtcNow)
        return AuthenticateResult.Fail("SESSION_EXPIRED");

      var isCurrent = await _sessions.IsCurrentAsync(rec.Value.UserId, sid);
      if (!isCurrent) return AuthenticateResult.Fail("SESSION_REVOKED");

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, rec.Value.UserId.ToString()),
        new Claim("sid", sid)
      };
      var identity = new ClaimsIdentity(claims, SchemeName);
      var principal = new ClaimsPrincipal(identity);
      var ticket = new AuthenticationTicket(principal, SchemeName);
      return AuthenticateResult.Success(ticket);
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
      // Biarkan Swagger lewat kalau memang tidak ingin diblok di sini
      if (Request.Path.StartsWithSegments("/swagger"))
        return base.HandleChallengeAsync(properties);

      if (!Response.HasStarted)
      {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        Response.ContentType = "application/json";
        return Response.WriteAsJsonAsync(new
        {
          IsSuccess = false,
          ErrorCode = "UNAUTHORIZED",
          ErrorMsg = "Tidak terautentikasi. Silakan login."
        });
      }
      return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
      if (Request.Path.StartsWithSegments("/swagger"))
        return base.HandleForbiddenAsync(properties);

      if (!Response.HasStarted)
      {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        Response.ContentType = "application/json";
        return Response.WriteAsJsonAsync(new
        {
          IsSuccess = false,
          ErrorCode = "FORBIDDEN",
          ErrorMsg = "Anda tidak memiliki izin untuk mengakses resource ini."
        });
      }
      return Task.CompletedTask;
    }

  }
}
