using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Data.Dao.MasterData
{
    public class User : BaseDao
    {
        public Pertamina? Pertamina { get; set; }
        public string NIK { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Telp { get; set; }
        public string? NIP { get; set; }
        public string? PDB { get; set; }
        public virtual Position? Position { get; set; }
        public string? PositionName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsLocked { get; set; } = false;
        public int WrongPss { get; set; }
        public FileUpload? Foto { get; set; }

    }
}
