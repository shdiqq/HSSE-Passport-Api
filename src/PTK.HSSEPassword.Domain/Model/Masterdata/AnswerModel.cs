using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
    public class AnswerDTModel : BaseDTModel
    {
        public int QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public string? Name { get; set; }
        public bool IsTrue { get; set; }
    }

    public class AnswerDTParamModel : BaseDTParamModel
    {
        public int QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public string? Name { get; set; }
        public bool IsTrue { get; set; }
    }

    public class AnswerModel : BaseModel
    {
        public int? QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public string? Name { get; set; }
        public bool? IsTrue { get; set; }
    }
}
