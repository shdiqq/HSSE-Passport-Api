namespace PTK.HSSEPassport.Api.Extensions
{
    public class APIKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "X-API-KEY";

        public APIKeyMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // ⬅️ Bebaskan Swagger & favicon agar UI jalan tanpa header
            var path = context.Request.Path;
            if (path.StartsWithSegments("/swagger") || path.StartsWithSegments("/favicon.ico"))
            {
                await _next(context);
                return;
            }

            var cfg = context.RequestServices.GetRequiredService<IConfiguration>();
            var keys = cfg.GetSection("APIKeys").Get<string[]>();

            if (keys == null || keys.Length == 0)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { IsSuccess = false, ErrorMsg = "Konfigurasi APIKeys tidak ditemukan." });
                }
                return; // ⬅️ WAJIB stop
            }

            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { IsSuccess = false, ErrorMsg = "Api Key was not provided" });
                }
                return; // ⬅️ WAJIB stop
            }

            if (!keys.Contains(extractedApiKey.ToString()))
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { IsSuccess = false, ErrorMsg = "Unauthorized client" });
                }
                return; // ⬅️ WAJIB stop
            }

            await _next(context);
        }

    }
}
