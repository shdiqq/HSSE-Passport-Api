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
    public class PassportDTModel : BaseDTModel
    {
        public string? PertaminaName { get; set; }
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? NIK { get; set; }
        public string? NIP { get; set; }
        public string? UserName { get; set; }
        public string? Date { get; set; }
        public string? Expired { get; set; }
        public FileUploadModel? FileSKCKData { get; set; }
        public FileUploadModel? FileMCUData { get; set; }
        public FileUploadModel? FileSBSSData { get; set; }
        public bool IsDemoRoom { get; set; }
        public string? DemoRoomDate { get; set; }
        public int PreTest { get; set; }
        public int PostTest { get; set; }
        public int Fraud { get; set; }
        public string? Status { get; set; }
    }

    public class PassportDTParamModel : BaseDTParamModel
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? NIK { get; set; }
        public string? Date { get; set; }
        public string? Expired { get; set; }
        //public FileUploadModel? FileSKCKData { get; set; }
        //public FileUploadModel? FileMCUData { get; set; }
        //public FileUploadModel? FileSBSSData { get; set; }
        public bool? IsDemoRoom { get; set; }
        public string? DemoRoomDate { get; set; }
        public int? PreTest { get; set; }
        public int? PostTest { get; set; }
        public int? Fraud { get; set; }
        public string? Status { get; set; }
        public int? Year { get; set; }
        public string? CreatedBy { get; set; }
        public string? Email { get; set; }
        public string? PertaminaId { get; set; }
    }

    public class PassportModel : BaseModel
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? NIK { get; set; }
        public DateOnly? Date { get; set; }
        public DateOnly? Expired { get; set; }
        public IFormFile? FileSKCKData { get; set; }
        public IFormFile? FileMCUData { get; set; }
        public IFormFile? FileSBSSData { get; set; }
        public bool? IsDemoRoom { get; set; }
        public DateOnly? DemoRoomDate { get; set; }
        public int? PreTest { get; set; }
        public int? PostTest { get; set; }
        public int? Fraud { get; set; }
        public string? Status { get; set; }
    }

    public class ChartDataModel
    {
        public string Name { get; set; }
        public bool ColorByPoint { get; set; }
        public List<ChartDataItem> Data { get; set; }
    }

    public class ChartDataItem
    {
        public string Name { get; set; }
        public double Y { get; set; }
        public bool Sliced { get; set; } = false; // Default false
        public bool Selected { get; set; } = false; // Default false
    }

    public class SeriesDataModel
    {
        public string Name { get; set; }
        public List<int> Data { get; set; }
    }


    public class ParamDashboardModel
    {
        public int Year { get; set; }
        public string? CreatedBy { get; set; }
    }
}
