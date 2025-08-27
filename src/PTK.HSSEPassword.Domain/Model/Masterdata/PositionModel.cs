using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
    public class PositionDTModel : BaseDTModel
    {
        public string? Name { get; set; }
    }

    public class PositionDTParamModel : BaseDTParamModel
    {
        public string? Name { get; set; }
    }

    public class PositionModel : BaseModel
    {
        public required string Name { get; set; }
    }
}
