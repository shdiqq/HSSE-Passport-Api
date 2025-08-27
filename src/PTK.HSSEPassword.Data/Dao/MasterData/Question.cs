using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Data.Dao.MasterData
{
    public class Question : BaseDao
    {
        public virtual Element? Element { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public FileUpload? Image { get; set; }

    }
}
