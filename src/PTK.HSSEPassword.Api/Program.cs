using Microsoft.OpenApi.Models;
using PTK.HSSEPassport.Api.Configurations;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Data;
using PTK.HSSEPassport.Api.Extensions;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Configurations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.ConfigureDatabaseConnection(Configuration);
builder.Services.AddCors();
builder.Services.ConfigureHTTPClientFactory(Configuration);
builder.Services.InjectDataLayer();
builder.Services.InjectDomainLayer();
//builder.Services.ConfigureMinio(Configuration);
builder.Services.ConfigureWhatsAppService(builder.Configuration);
builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "Version 1.0",
        Title = "HSSEPassport API",
        Contact = new OpenApiContact
        {
            Name = "PTK Dev Team",
            Email = "dev.ptk@pertamina.com",
            Url = new Uri("https://www.pertamina-ptk.com/")
        }
    });

    opt.AddSecurityDefinition("API Key", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey
    });

    //opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    Name = "JWT Authentication",
    //    Description = "Enter JWT Bearer token **_only_**",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.Http,
    //    Scheme = "bearer", // must be lower case
    //    BearerFormat = "JWT",
    //});

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = "API Key"
                        },
                        Name = "X-API-KEY",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header
                    },
                        new List<string>()
                    }
                });

    //opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    //            {
    //                { new OpenApiSecurityScheme
    //                    {
    //                        Reference = new OpenApiReference
    //                        {
    //                            Type = ReferenceType.SecurityScheme,
    //                            Id = "Bearer"
    //                        }
    //                    },
    //                    new string[] { }
    //                }
    //            });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
});


builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("Database"));
GeneralConstant.Initialize(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseDeveloperExceptionPage();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<APIKeyMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
