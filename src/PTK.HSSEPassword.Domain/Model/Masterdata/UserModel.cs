using Microsoft.AspNetCore.Http;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Domain.Models.MasterData
{
    public class UserDTModel : BaseDTModel
    {
        public int? PertaminaId { get; set; }
        public string? PertaminaName { get; set; }
        public string? PertaminaCode { get; set; }
        public string? NIK { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Telp { get; set; }
        public string? NIP { get; set; }
        public string? PDB { get; set; }
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? IsLocked { get; set; }
        public int? WrongPassword { get; set; }
        public FileUploadModel? FotoData { get; set; }
    }

    public class UserDTParamModel : BaseDTParamModel
    {
        public int? PertaminaId { get; set; }
        public string? PertaminaName { get; set; }
        public string? PertaminaCode { get; set; }
        public string? NIK { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Telp { get; set; }
        public string? NIP { get; set; }
        public string? PDB { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool? IsLocked { get; set; }
        public int? WrongPassword { get; set; }
    }

    public class UserModel : BaseModel
    {
        public int? PertaminaId { get; set; }
        public string? PertaminaName { get; set; }
        public string? PertaminaCode { get; set; }
        public string? NIK { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Telp { get; set; }
        public string? NIP { get; set; }
        public string? PDB { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool IsLocked { get; set; }
        public int? WrongPassword { get; set; }
        public IFormFile? FotoData { get; set; }
        public bool IsIdaman { get; set; }
        public string? PasswordNew { get; set; }
    }
}
