using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Data.Dao.Transaction
{
    public class TestDetail : BaseDao
    {
        public virtual Test? Test { get; set; }
        public virtual Question? Question { get; set; }
        public virtual Answer? Answer { get; set; }
        public int Score { get; set; }
        public int Total { get; set; }
        public bool IsTrue { get; set; }
    }
}
