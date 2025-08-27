using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Data.Dao.Transaction
{
    public class Test : BaseDao
    {
        public virtual Passport? Passport { get; set; }
        public DateOnly Date { get; set; }
        public string Flag { get; set; }
        public int Score { get; set; }
    }
}
