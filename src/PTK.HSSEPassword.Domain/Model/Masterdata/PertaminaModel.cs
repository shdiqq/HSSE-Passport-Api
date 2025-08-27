using Microsoft.AspNetCore.Http;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
    public class PertaminaDTModel : BaseDTModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public FileUploadModel? LogoData { get; set; }

        public string? OfficialName { get; set; }
        public string? OfficialPosition { get; set; }
        public FileUploadModel? OfficialSignData { get; set; }
    }

    public class PertaminaDTParamModel : BaseDTParamModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public string? OfficialName { get; set; }
        public string? OfficialPosition { get; set; }
    }

    public class PertaminaModel : BaseModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public IFormFile? LogoData { get; set; }

        public string? OfficialName { get; set; }
        public string? OfficialPosition { get; set; }
        public IFormFile? OfficialSignData { get; set; }
    }
}
