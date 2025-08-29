using Microsoft.OpenApi.Models;

using PTK.HSSEPassport.Api.Configurations;
using PTK.HSSEPassport.Api.Extensions;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Configurations;
using PTK.HSSEPassport.Data.Service.Session;
using PTK.HSSEPassport.Data.Service.Session.Impl;

using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using PTK.HSSEPassword.Api.Auth;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

// Session store untuk single-active-session
builder.Services.AddScoped<ISessionStoreService, SessionStoreServiceImpl>();

// Koneksi DB, CORS, HTTP Client, dsb (sesuai existing)
builder.Services.ConfigureDatabaseConnection(Configuration);
builder.Services.AddCors(
// opt =>
// {
//     opt.AddPolicy("FrontEnd", p => p
//     .WithOrigins(
//         "https://ptmpisappdev01.pertamina.com/",
//         "http://localhost:5209/"
//     )
//     .AllowAnyHeader()
//     .AllowAnyMethod()
//     .AllowCredentials()
//     );
// }
);
builder.Services.ConfigureHTTPClientFactory(Configuration);
builder.Services.InjectDataLayer();
builder.Services.InjectDomainLayer();
//builder.Services.ConfigureMinio(Configuration);
builder.Services.ConfigureWhatsAppService(builder.Configuration);

// Controllers + JSON
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services
  .AddAuthentication(options =>
  {
      options.DefaultScheme = SessionAuthenticationHandler.SchemeName;
      options.DefaultAuthenticateScheme = SessionAuthenticationHandler.SchemeName;
      options.DefaultChallengeScheme = SessionAuthenticationHandler.SchemeName;
  })
  .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(
      SessionAuthenticationHandler.SchemeName, _ => { });

// ===== Swagger =====
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

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
});

// AppSetting (existing)
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseCors("FrontEnd");

app.UseMiddleware<APIKeyMiddleware>();

app.MapGet("/swagger/{**any}", () => Results.Redirect("/swagger/index.html")).AllowAnonymous();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
