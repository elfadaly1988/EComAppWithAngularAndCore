using ECom.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace ECom.Api.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate _next { get; }
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);
                if (!IsRequestAllowed(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new
                        APIException((int)HttpStatusCode.TooManyRequests, "Too many requests . Please Try Again Later");
                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _environment.IsDevelopment() ?
                    new APIException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new APIException((int)HttpStatusCode.InternalServerError, ex.Message);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
        private bool IsRequestAllowed(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress.ToString();
            var cachKey = $"Rate:{ipAddress}";
            var dateNow = DateTime.Now;
            //Using memory cache to store the request
            var (timesTamp, count) = _memoryCache.GetOrCreate(cachKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timeStamp: dateNow, count: 0);

            });
            if (dateNow - timesTamp <= _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }
                _memoryCache.Set(cachKey, (timesTamp, count + 1), _rateLimitWindow);
                return true;
            }
            else
            {
                _memoryCache.Set(cachKey, (timesTamp, count), _rateLimitWindow);
                return true;
            }


        }
        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
        }
    }

}

