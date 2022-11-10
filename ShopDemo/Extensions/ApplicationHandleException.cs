using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace ShopDemo.Web.Extensions
{
    public static class ApplicationHandleException
    {
        public static void UseExceptionHandlerCustom(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature?.Error;
                    var innnerException = new Exception();
                    if (exception is AggregateException aggregateException)
                    {
                        aggregateException.Handle(inner =>
                        {
                            innnerException = inner;
                            return true;
                        });
                    }
                    else
                    {
                        innnerException = exception;
                    }
                    await HandleException(context, innnerException, logger);
                });
            });
        }

        private static async Task HandleException(HttpContext context, Exception exception, ILogger logger)
        {
            var response = new
            {
                message = "Server error, please contact to admin",
                status = "Error"
            };
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }), Encoding.UTF8);
            logger.LogError(exception, exception.Message + exception.StackTrace);
        }
    }
}
