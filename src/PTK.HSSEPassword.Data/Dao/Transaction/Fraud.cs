using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Data.Dao.Transaction
{
    public class Fraud : BaseDao
    {
        public DateOnly Date { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public virtual Passport? Passport { get; set; }
        public FileUpload? FileFraud { get; set; }
    }
}
