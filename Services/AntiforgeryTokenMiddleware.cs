namespace PictIt.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Http;

    public class AntiforgeryTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public AntiforgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/")
            {
                var tokens = _antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
            }
            return _next(context);
        }
    }
}
