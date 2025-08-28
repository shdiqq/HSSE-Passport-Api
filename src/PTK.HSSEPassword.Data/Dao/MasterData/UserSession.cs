using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTK.HSSEPassport.Api.Data.Dao.Sessions
{
  [Table("UserSessions", Schema = "dbo")]
  public class UserSession
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required, Column(TypeName = "char(32)")]
    public string SessionId { get; set; } = default!;

    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    [Column(TypeName = "nvarchar(200)")]
    public string? DeviceInfo { get; set; }
    public bool IsActive { get; set; }

  }
}
