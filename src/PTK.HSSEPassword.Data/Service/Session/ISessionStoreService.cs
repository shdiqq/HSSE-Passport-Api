namespace PTK.HSSEPassport.Data.Service.Session
{
  public interface ISessionStoreService
  {
    Task SetCurrentSessionAsync(int userId, string sessionId, DateTime? expiresAt, string? deviceInfo = null);
    Task<string?> GetCurrentSessionAsync(int userId);
    Task<bool> IsCurrentAsync(int userId, string sessionId);
    Task RevokeCurrentAsync(int userId);
    Task RevokeSessionAsync(int userId, string sessionId);
  }
}
