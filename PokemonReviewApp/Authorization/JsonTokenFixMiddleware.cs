using System.Text.Json;
namespace PokemonReviewApp.Authorization


{
    public class JsonTokenFixMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonTokenFixMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader))
            {
                // Eğer header JSON gibi görünüyorsa:
                if (authHeader.TrimStart().StartsWith("{"))
                {
                    try
                    {
                        var json = JsonDocument.Parse(authHeader);
                        var token = json.RootElement.GetProperty("token").GetString();

                        if (!string.IsNullOrEmpty(token))
                        {
                            // Authorization header'ı sadece token yap
                            context.Request.Headers["Authorization"] = token;
                        }
                    }
                    catch
                    {
                        // JSON değilse hiçbir şey yapma
                    }
                }
            }

            await _next(context);
        }
    }
}
