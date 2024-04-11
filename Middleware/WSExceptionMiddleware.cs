using Serilog;
using Serilog.AspNetCore;
using System.Text;

namespace Andyyy.Middleware
{
    public class WSExceptionMiddleware
    {
        private readonly ILogger<WSExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        public WSExceptionMiddleware(ILogger<WSExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // 攔截請求
            var request = await FormatRequest(context.Request);

            // 紀錄回應前的狀態


            await _next(context);


            // 紀錄資訊
            _logger.LogInformation("Request: \n{Request}", request);


        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var headers = FormatHeaders(request.Headers);

            return $"HttpMethod: {request.Method}\n" +
                   $"RequestPath: {request.Path}\n" +
                   $"RequestQueryString: {request.QueryString}\n" +
                   $"Headers: \n{headers}";
        }

        private string FormatHeaders(IHeaderDictionary headers)
        {
            var formattedHeaders = new StringBuilder();
            foreach (var (key, value) in headers)
            {
                formattedHeaders.AppendLine($"\t{key}: {string.Join(",", value)}");
            }

            return formattedHeaders.ToString();
        }
    }
}
