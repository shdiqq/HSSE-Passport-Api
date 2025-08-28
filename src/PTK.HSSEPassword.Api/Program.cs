using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using PTK.HSSEPassport.Api.Configurations;
using PTK.HSSEPassport.Api.Extensions;
using PTK.HSSEPassport.Api.Utilities.Constants;
using PTK.HSSEPassport.Utilities.Configurations;
using PTK.HSSEPassport.Data.Service.Session;
using PTK.HSSEPassport.Data.Service.Session.Impl;

using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

// Session store untuk single-active-session
builder.Services.AddScoped<ISessionStoreService, SessionStoreServiceImpl>();

// Koneksi DB, CORS, HTTP Client, dsb (sesuai existing)
builder.Services.ConfigureDatabaseConnection(Configuration);
builder.Services.AddCors();
builder.Services.ConfigureHTTPClientFactory(Configuration);
builder.Services.InjectDataLayer();
builder.Services.InjectDomainLayer();
//builder.Services.ConfigureMinio(Configuration);
builder.Services.ConfigureWhatsAppService(builder.Configuration);

// Controllers + JSON
builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

// ===== JWT Authentication =====
var jwtKey = Configuration["Jwt:Key"];
var jwtIssuer = Configuration["Jwt:Issuer"];

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opt =>
  {
      opt.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
          ValidateIssuer = true,
          ValidIssuer = jwtIssuer,     // harus sama dgn "iss" di token
          ValidateAudience = false,
          ValidateLifetime = false,    // ← penting: TANPA exp
          ClockSkew = TimeSpan.Zero
      };

      // Hook: cek single active session setiap request
      opt.Events = new JwtBearerEvents
      {
          OnTokenValidated = async ctx =>
          {
              var store = ctx.HttpContext.RequestServices.GetRequiredService<ISessionStoreService>();

              var sub = ctx.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? ctx.Principal?.FindFirst("sub")?.Value;
              var jti = ctx.Principal?.FindFirst("jti")?.Value;

              if (!int.TryParse(sub, out var userId) || string.IsNullOrEmpty(jti) ||
                  !await store.IsCurrentAsync(userId, jti))
              {
                  // tandai agar OnChallenge mengembalikan 401 yang rapi
                  ctx.Fail("SESSION_REVOKED");
              }
          },

          OnChallenge = async context =>
          {
              if (context.Response.HasStarted)
              {
                  context.HandleResponse();
                  return;
              }

              if (context.HttpContext.Request.Path.StartsWithSegments("/swagger"))
              {
                  context.HandleResponse();
                  return;
              }

              context.HandleResponse();
              var err = context.AuthenticateFailure?.Message ?? "UNAUTHORIZED";
              context.Response.StatusCode = StatusCodes.Status401Unauthorized;
              context.Response.ContentType = "application/json";
              context.Response.Headers["WWW-Authenticate"] =
                  $"Bearer error=\"invalid_token\", error_description=\"{err}\"";
              context.Response.Headers["X-Auth-Error"] = err;

              await context.Response.WriteAsJsonAsync(new
              {
                  IsSuccess = false,
                  ErrorCode = err,
                  ErrorMsg = err == "SESSION_REVOKED"
                ? "Sesi Anda telah dialihkan ke perangkat lain. Silakan login kembali."
                : "Tidak terautentikasi. Silakan login."
              });
          },

          OnForbidden = async context =>
              {
                  if (context.Response.HasStarted)
                  {
                      // tidak perlu HandleResponse di Forbidden, tapi aman juga kalau ditambah
                      return;
                  }
                  if (context.HttpContext.Request.Path.StartsWithSegments("/swagger")) return;

                  context.Response.StatusCode = StatusCodes.Status403Forbidden;
                  context.Response.ContentType = "application/json";
                  await context.Response.WriteAsJsonAsync(new
                  {
                      IsSuccess = false,
                      ErrorCode = "FORBIDDEN",
                      ErrorMsg = "Anda tidak memiliki izin untuk mengakses resource ini."
                  });
              },

          OnAuthenticationFailed = async context =>
              {
                  if (context.Response.HasStarted)
                  {
                      // boleh stop saja; default tidak akan menulis lagi
                      return;
                  }
                  if (context.HttpContext.Request.Path.StartsWithSegments("/swagger")) return;

                  context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                  context.Response.ContentType = "application/json";
                  await context.Response.WriteAsJsonAsync(new
                  {
                      IsSuccess = false,
                      ErrorCode = "TOKEN_INVALID",
                      ErrorMsg = "Token tidak valid atau rusak. Silakan login lagi."
                  });
              }
      };
  });

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

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Masukkan JWT tanpa kata 'Bearer'.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
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

app.UseMiddleware<APIKeyMiddleware>();   // ⬅️ API Key dulu & short-circuit

app.MapGet("/swagger/{**any}", () => Results.Redirect("/swagger/index.html")).AllowAnonymous();

app.UseAuthentication();                 // baru JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
