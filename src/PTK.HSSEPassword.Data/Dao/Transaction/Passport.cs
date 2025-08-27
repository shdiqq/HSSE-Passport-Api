using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Data.Dao.Transaction
{
    public class Passport : BaseDao
    {
        public virtual User User { get; set; }
        public DateOnly Date { get; set; }
        public DateOnly? Expired { get; set; }
        public FileUpload? FileSKCK { get; set; }
        public FileUpload? FileMCU { get; set; }
        public FileUpload? FileSBSS { get; set; }
        public bool IsDemoRoom { get; set; }
        public DateOnly? DemoRoomDate { get; set; }
        public int PreTest { get; set; }
        public int PostTest { get; set; }
        public int Fraud { get; set; }
        public string Status { get; set; }

    }
}
