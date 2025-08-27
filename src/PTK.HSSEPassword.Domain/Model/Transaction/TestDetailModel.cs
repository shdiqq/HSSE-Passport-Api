using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.Transaction
{
    public class TestDetailDTModel : BaseDTModel
    {
        public string? TestId { get; set; }
        public string? TestName { get; set; }
        public string? QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public string? AnswerId { get; set; }
        public string? AnswerName { get; set; }
        public int? Score { get; set; }
        public int? Total { get; set; }
        public bool IsTrue { get; set; }
    }

    public class TestDetailDTParamModel : BaseDTParamModel
    {
        public string? TestId { get; set; }
        public string? TestName { get; set; }
        public string? QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public string? AnswerId { get; set; }
        public string? AnswerName { get; set; }
        public int? Score { get; set; }
        public int? Total { get; set; }
        public bool IsTrue { get; set; }
    }

    public class TestDetailModel : BaseModel
    {
        public string? TestId { get; set; }
        public string? TestName { get; set; }
        public string? QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public string? AnswerId { get; set; }
        public string? AnswerName { get; set; }
        public int? Score { get; set; }
        public int? Total { get; set; }
        public bool IsTrue { get; set; }
    }
}
