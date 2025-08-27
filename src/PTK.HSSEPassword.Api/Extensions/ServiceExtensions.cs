using Microsoft.EntityFrameworkCore;
using Praise.Domain.Impl.Praise.MasterData;
using Praise.Domain.Impl.Praise.Transaction;
using PTK.HSSEPassport.Api.Configurations;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Domain.Impl;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata;
using PTK.HSSEPassport.Api.Domain.Interfaces.Transaction;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Data.Service.Upload;
using PTK.HSSEPassport.Data.Service.Upload.Impl;
using static PTK.HSSEPassport.Api.Utilities.Base.BaseEnum;



namespace PTK.HSSEPassport.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var dbMode = configuration.GetValue<string>("Database:Mode");
            var connection = configuration.GetValue<string>($"Database:ConnectionStrings:{dbMode}");

            services.AddDbContext<HSSEPassportDbContext>(options
                => options.UseSqlServer(connection + DatabaseEnums.HSSEPasspostDb.ToString()));
        }

        //public static void ConfigureMinio(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var minioMode = configuration.GetValue<string>("MinioService:Mode");
        //    services.AddMinio(opt =>
        //    {
        //        opt.Endpoint = configuration.GetValue<string>($"MinioService:URL:{minioMode}");
        //        opt.AccessKey = configuration.GetValue<string>($"MinioService:AccessKey");
        //        opt.SecretKey = configuration.GetValue<string>($"MinioService:SecretKey");

        //    });
        //}
        public static void ConfigureHTTPClientFactory(this IServiceCollection services, IConfiguration configuration)
        {
            var httpMode = configuration.GetValue<string>($"BaseUrl:{ClientConstant.COREAPI}:Mode");
            services.AddHttpClient(ClientConstant.COREAPI, (httpClient) =>
            {
                var baseUrl = configuration.GetValue<string>($"BaseUrl:{ClientConstant.COREAPI}:Url:{httpMode}");
                var apiKey = configuration.GetValue<string>($"BaseUrl:{ClientConstant.COREAPI}:APIKey:{httpMode}");
                httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
                httpClient.BaseAddress = new Uri(baseUrl);
            });
        }



        public static void InjectDataLayer(this IServiceCollection services)
        {
            //services.AddScoped<ShipRepo>();
            services.AddScoped<IUploadService<FileUpload>, UploadServiceImpl<FileUpload, HSSEPassportDbContext>>();
        }

        public static void InjectDomainLayer(this IServiceCollection services)
        {
            services.AddScoped<IAppLogService, AppLogServiceImpl>();
            services.AddScoped<IFileService, FileServiceImpl>();


            services.AddScoped<IPertaminaService, PertaminaImpl>();
            services.AddScoped<IAnswerService, AnswerImpl>();
            services.AddScoped<IConfigService, ConfigImpl>();
            services.AddScoped<IPositionService, PositionImpl>();
            services.AddScoped<IElementService, ElementImpl>();
            services.AddScoped<IQuestionService, QuestionImpl>();
            services.AddScoped<IUserService, UserImpl>();

            services.AddScoped<IFraudService, FraudImpl>();
            services.AddScoped<IPassportService, PassportImpl>();
            services.AddScoped<ITestService, TestImpl>();
            services.AddScoped<ITestDetailService, TestDetailImpl>();

        }
    }
}
