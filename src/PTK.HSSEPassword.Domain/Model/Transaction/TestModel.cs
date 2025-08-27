using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.Transaction
{
    public class TestDTModel : BaseDTModel
    {
        public int? PassportId { get; set; }
        public string? PassportName { get; set; }
        public DateOnly? Date { get; set; }
        public string? Flag { get; set; }
        public int? Score { get; set; }
    }

    public class TestDTParamModel : BaseDTParamModel
    {
        public int? PassportId { get; set; }
        public string? PassportName { get; set; }
        public DateOnly? Date { get; set; }
        public string? Flag { get; set; }
        public int? Score { get; set; }
    }

    public class TestModel : BaseModel
    {
        public int? PassportId { get; set; }
        public string? PassportName { get; set; }
        public DateOnly Date { get; set; }
        public string? Flag { get; set; }
        public int? Score { get; set; }
        public List<QuestionModel>? Question { get; set; }
    }
}
