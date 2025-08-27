namespace PTK.HSSEPassport.Api.Extensions
{
    public class APIKeyMiddleware
    {
        private readonly RequestDelegate _next;

        private const string APIKEYNAME = "X-API-KEY";

        public APIKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            IConfiguration requiredService = context.RequestServices.GetRequiredService<IConfiguration>();
            if (!requiredService.GetSection("APIKeys").Exists())
            {
                throw new NotImplementedException("Tambahkan daftar konfigurasi APIKeys di appsettings.json");
            }

            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided ");
            }
            else if (requiredService.GetSection("APIKeys").Get<string[]>().FirstOrDefault((string b) => b == extractedApiKey) == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
