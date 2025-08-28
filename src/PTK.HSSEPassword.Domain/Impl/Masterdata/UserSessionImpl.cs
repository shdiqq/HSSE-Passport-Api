using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Data.Service.Session;

namespace PTK.HSSEPassport.Api.Domain.Impl.MasterData
{
  public class UserSessionImpl
  {
    private readonly ISessionStoreService _sessionStore;

    public UserSessionImpl(ISessionStoreService sessionStore)
    {
      _sessionStore = sessionStore;
    }

    public async Task<UserSessionModel?> GetCurrentSessionAsync(int userId)
    {
      var sessionId = await _sessionStore.GetCurrentSessionAsync(userId);
      if (string.IsNullOrEmpty(sessionId)) return null;

      return new UserSessionModel
      {
        UserId = userId,
        SessionId = sessionId,
        IssuedAt = DateTime.UtcNow,
        IsActive = true
      };
    }

    public async Task<bool> ValidateSessionAsync(int userId, string sessionId)
    {
      return await _sessionStore.IsCurrentAsync(userId, sessionId);
    }

    public async Task RevokeAsync(int userId, string sessionId)
    {
      await _sessionStore.RevokeSessionAsync(userId, sessionId);
    }
  }
}
