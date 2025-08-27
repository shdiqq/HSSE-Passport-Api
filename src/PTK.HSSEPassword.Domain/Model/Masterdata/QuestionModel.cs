using Microsoft.AspNetCore.Http;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
    public class QuestionDTModel : BaseDTModel
    {
        public string? Name { get; set; }
        public int? Score { get; set; }
        public ElementModel? Element { get; set; }
        public int? ElementId { get; set; }
        public string? ElementName { get; set; }
        public List<AnswerModel>? Answer { get; set; }
        public FileUploadModel? ImageData { get; set; }
    }

    public class QuestionDTParamModel : BaseDTParamModel
    {
        public string? Name { get; set; }
        public int? Score { get; set; }
        public ElementModel? Element { get; set; }
        public int? ElementId { get; set; }
        public string? ElementName { get; set; }
    }

    public class QuestionModel : BaseModel
    {
        public string? Name { get; set; }
        public int? Score { get; set; }
        public ElementModel? Element { get; set; }
        public int? ElementId { get; set; }
        public string? ElementName { get; set; }
        public List<AnswerModel>? Answer { get; set; }
        public IFormFile? ImageData { get; set; }
        public FileUploadModel? Image { get; set; }
        public List<TestDetailModel>? TestDetail { get; set; }
    }
}
