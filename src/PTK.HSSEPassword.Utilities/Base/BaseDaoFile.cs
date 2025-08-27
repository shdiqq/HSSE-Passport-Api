using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Utilities.Base
{
    public class BaseDaoFile : BaseDao
    {
        public string BucketName { get; set; } = "uploaded";


        public string? FilePath { get; set; }

        public string? FileName { get; set; }

        [StringLength(100)]
        public string? ContentType { get; set; }

        public long Length { get; set; }

        public int TrxId { get; set; }

        public string? Flag { get; set; }

        public string? Remark { get; set; }
    }
}
