
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Data.Dao.MasterData
{
    public class Answer : BaseDao
    {
        public virtual Question? Question { get; set; }
        public string Name { get; set; }
        public bool IsTrue { get; set; }

    }
}
