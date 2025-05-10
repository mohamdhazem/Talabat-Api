using System.Net;
using System.Text.Json;
using Talabat.APIs.HandlingErrors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment hostEnvironment;

        // by convention
        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IHostEnvironment hostEnvironment)
        {
            this.next = next;
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                // 1- log exception 
                logger.LogError(ex , ex.Message); // for Development

                // 2- return response to client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // i want to create object from ApiExceptionResponse
                var Response = hostEnvironment.IsDevelopment()?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError , ex.Message , ex.StackTrace?.ToString())
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                // i want to serialize the object to json to send it to WriteAsync and make it in CamelCase
                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(Response, option);
                
                await context.Response.WriteAsync(json);
            }
        }
    }
}
