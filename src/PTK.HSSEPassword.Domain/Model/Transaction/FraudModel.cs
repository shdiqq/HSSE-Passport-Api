using Microsoft.AspNetCore.Http;
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
    public class FraudDTModel : BaseDTModel
    {
        public DateOnly? Date { get; set; }
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? NIK { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
        public int? PassportId { get; set; }
        public string? PassportName { get; set; }
        public FileUploadModel? FileUploadData { get; set; }
    }

    public class FraudDTParamModel : BaseDTParamModel
    {
        public DateOnly? Date { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? NIK { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
        public int? PassportId { get; set; }
        public string? PassportName { get; set; }
        public FileUploadModel? FileUploadData { get; set; }

        public string? CreatedBy { get; set; }
    }

    public class FraudModel : BaseModel
    {
        public DateOnly Date { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? NIK { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
        public int? PassportId { get; set; }
        public string? PassportName { get; set; }
        public IFormFile? FileUploadData { get; set; }
    }
}
