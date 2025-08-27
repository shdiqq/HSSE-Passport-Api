using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
    public class ElementDTModel : BaseDTModel
    {
        public string? Name { get; set; }
    }

    public class ElementDTParamModel : BaseDTParamModel
    {
        public string? Name { get; set; }
    }

    public class ElementModel : BaseModel
    {
        public required string Name { get; set; }
        public List<QuestionModel>? Question { get; set; }
    }
}
