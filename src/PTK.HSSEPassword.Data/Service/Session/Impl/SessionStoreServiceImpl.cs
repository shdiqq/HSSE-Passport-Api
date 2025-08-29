using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Data;

namespace PTK.HSSEPassport.Data.Service.Session.Impl
{
  public sealed class SessionStoreServiceImpl(HSSEPassportDbContext db) : ISessionStoreService
  {
    private readonly HSSEPassportDbContext _db = db;

    public async Task SetCurrentSessionAsync(int userId, string sessionId, DateTime? expiresAt, string? deviceInfo = null)
    {
      await using var tx = await _db.Database.BeginTransactionAsync();

      // Nonaktifkan sesi aktif sebelumnya
      await _db.Database.ExecuteSqlInterpolatedAsync($@"
        UPDATE dbo.UserSessions SET IsActive = 0
        WHERE UserId = {userId} AND IsActive = 1;");

      // Tambahkan sesi baru
      await _db.Database.ExecuteSqlInterpolatedAsync($@"
        INSERT INTO dbo.UserSessions(UserId, SessionId, IssuedAt, ExpiresAt, DeviceInfo, IsActive)
        VALUES ({userId}, {sessionId}, SYSUTCDATETIME(), {expiresAt}, {deviceInfo}, 1);");

      await tx.CommitAsync();
    }

    public async Task<string?> GetCurrentSessionAsync(int userId)
      => await _db.UserSessions
        .AsNoTracking()
        .Where(s => s.UserId == userId && s.IsActive)
        .OrderByDescending(s => s.IssuedAt)
        .Select(s => s.SessionId)
        .FirstOrDefaultAsync();

    public async Task<bool> IsCurrentAsync(int userId, string sessionId)
      => (await GetCurrentSessionAsync(userId)) == sessionId;

    public async Task RevokeCurrentAsync(int userId)
      => await _db.Database.ExecuteSqlInterpolatedAsync($@"
        UPDATE dbo.UserSessions SET IsActive = 0
        WHERE UserId = {userId} AND IsActive = 1;");

    public async Task RevokeSessionAsync(int userId, string sessionId)
      => await _db.Database.ExecuteSqlInterpolatedAsync($@"
        UPDATE dbo.UserSessions SET IsActive = 0
        WHERE UserId = {userId} AND SessionId = {sessionId};");

    // Lookup cepat by sessionId (untuk auth handler cookie)
    public async Task<(int UserId, DateTime IssuedAt, DateTime? ExpiresAt, bool IsActive)?> FindBySessionAsync(string sessionId)
    {
      var s = await _db.UserSessions
        .AsNoTracking()
        .Where(x => x.SessionId == sessionId)
        .Select(x => new { x.UserId, x.IssuedAt, x.ExpiresAt, x.IsActive })
        .FirstOrDefaultAsync();

      return s is null ? null : (s.UserId, s.IssuedAt, s.ExpiresAt, s.IsActive);
    }
  }
}
