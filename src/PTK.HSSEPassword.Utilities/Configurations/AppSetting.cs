using JetBrains.Annotations;

namespace PTK.HSSEPassport.Utilities.Configurations
{
    [UsedImplicitly]
    public class AppSetting
    {
        public string Mode { get; set; } // karena "Mode": "Prod"
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public IDictionary<string, string> DataBase { get; set; }
        public bool IsProduction { get; set; }
        public bool IsLocalDevelopment { get; set; }
        public IDictionary<string, string> Secret { get; set; }
        public IDictionary<string, string> BaseURL { get; set; }
        public Dictionary<string, string> UrlUpload { get; set; }
    }
}
