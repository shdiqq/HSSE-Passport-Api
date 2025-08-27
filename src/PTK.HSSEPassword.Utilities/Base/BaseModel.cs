using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Utilities.Base
{
    public class BaseModel
    {
        public int? Id { get; set; }
        public bool IsActive { get; set; } = false;
        public string? CreatedBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
