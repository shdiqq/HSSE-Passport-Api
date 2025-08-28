using PTK.HSSEPassport.Api.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
  public class UserSessionModel : BaseModel
  {
    public int UserId { get; set; }                 // FK ke User.Id (INT)
    public string SessionId { get; set; } = default!; // char(32)
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? DeviceInfo { get; set; }
  }

  // Opsional: untuk listing DataTable
  public class UserSessionDTModel : BaseDTModel
  {
    public int UserId { get; set; }
    public string SessionId { get; set; } = default!;
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? DeviceInfo { get; set; }
  }

  // Opsional: parameter filter untuk listing/pencarian
  public class UserSessionDTParamModel : BaseDTParamModel
  {
    public int? UserId { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? IssuedFrom { get; set; }
    public DateTime? IssuedTo { get; set; }
    public string? DeviceContains { get; set; }
  }
}
