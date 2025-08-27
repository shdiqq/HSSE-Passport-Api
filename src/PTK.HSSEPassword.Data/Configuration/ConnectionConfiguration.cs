using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Data;
using static PTK.HSSEPassport.Api.Utilities.Base.BaseEnum;

namespace PTK.HSSEPassport.Data.Configuration
{
    public class ConnectionConfiguration
    {
        public static void GetService(IServiceCollection services, IConfiguration configuration)
        {
            var mode = configuration.GetValue<string>("Database:Mode");
            var connection = configuration.GetValue<string>($"Database:ConnectionStrings:{mode}");

			services.AddDbContext<HSSEPassportDbContext>(options 
                => options.UseSqlServer(connection + DatabaseEnums.HSSEPasspostDb.ToString()));
        }
    }
}
