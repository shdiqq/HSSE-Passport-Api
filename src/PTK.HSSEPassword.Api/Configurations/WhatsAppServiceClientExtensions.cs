using PTK.HSSEPassport.Data.Service.Whatsapp;
using PTK.HSSEPassport.Data.Service.Whatsapp.Impl;

namespace PTK.HSSEPassport.Api.Configurations
{
    public static class WhatsAppServiceClientExtensions
    {
        public static IServiceCollection ConfigureWhatsAppService(this IServiceCollection services, IConfiguration configuration)
        {
            IConfiguration configuration2 = configuration;
            if (!configuration2.GetSection("BaseUrl").Exists())
            {
                throw new NotImplementedException("Tambahkan konfigurasi 'BaseUrl' di appsettings.json");
            }

            if (!configuration2.GetSection("BaseUrl:WA").Exists())
            {
                throw new NotImplementedException("Tambahkan 'WA' pada konfigurasi section 'BaseUrl' di appsettings.json");
            }

            if (!configuration2.GetSection("BaseUrl:WA:Url").Exists())
            {
                throw new NotImplementedException("Tambahkan 'Url' pada konfigurasi section 'WA' di appsettings.json");
            }

            if (!configuration2.GetSection("BaseUrl:WA:Mode").Exists())
            {
                throw new NotImplementedException("Tambahkan 'Mode' pada konfigurasi section 'WA' di appsettings.json");
            }

            services.AddHttpClient("WA.SERVICE", delegate (HttpClient o)
            {
                string value = configuration2.GetValue<string>("BaseUrl:WA:Mode");
                if (!configuration2.GetSection("BaseUrl:WA:Url:" + value).Exists())
                {
                    throw new NotImplementedException("Mode '" + value + "' di url pada konfigurasi section WA BaseUrl di appsettings.json tidak ditemukan");
                }

                o.BaseAddress = new Uri(configuration2.GetValue<string>("BaseUrl:WA:Url:" + value));
            });
            services.AddScoped<IWhatsAppClientService, WhatsAppClientServiceImpl>();
            return services;
        }
    }
}
