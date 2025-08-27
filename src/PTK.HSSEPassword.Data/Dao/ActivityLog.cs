using PTK.HSSEPassport.Api.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace PTK.HSSEPassport.Api.Data.Dao
{
    public class ActivityLog
    {
        [Required]
        [StringLength(40)]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        public string? TrxId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string? UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string? Controller { get; set; } = string.Empty;

        [StringLength(40)]
        public string? Method { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string? Status { get; set; } = string.Empty;

        [StringLength(400)]
        public string? Info { get; set; } = GeneralConstant.GetLocalIPAddress();

        public string? ErrorMessage { get; set; } = string.Empty;

        [StringLength(400)]
        public string? Remark { get; set; } = string.Empty;
    }
}
