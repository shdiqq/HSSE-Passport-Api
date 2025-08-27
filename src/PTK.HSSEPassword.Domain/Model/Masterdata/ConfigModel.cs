using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
    public class ConfigDTModel : BaseDTModel
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
    }

    public class ConfigDTParamModel : BaseDTParamModel
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
    }

    public class ConfigModel : BaseModel
    {
        public required string Name { get; set; }
        public string? Note { get; set; }
    }
}
