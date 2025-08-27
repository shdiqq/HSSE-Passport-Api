using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models
{
    public class FileUploadDTModel : BaseDTModel
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
    }

    public class FileUploadDTParamModel : BaseDTParamModel
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
    }

    public class FileUploadModel : BaseModel
    {
        public string BucketName { get; set; } = "uploaded";
        public string FilePath { get; set; }

        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public long Length { get; set; }

        public int TrxId { get; set; }

        public string Flag { get; set; }

        public string Remark { get; set; }
    }
}
