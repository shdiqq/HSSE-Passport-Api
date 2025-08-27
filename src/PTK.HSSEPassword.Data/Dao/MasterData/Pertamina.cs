using PTK.HSSEPassport.Utilities.Base;


namespace PTK.HSSEPassport.Api.Data.Dao.MasterData
{
    public class Pertamina : BaseDao
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public FileUpload? Logo { get; set; }

        public string OfficialName { get; set; }
        public string OfficialPosition { get; set; }
        public FileUpload? OfficialSign { get; set; }

    }
}
