using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLive;

        public CacheAttribute(int timeToLive)
        {
            _timeToLive = timeToLive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Ask CLR To inject the service explicitly not impliciltliy in the constructor 
            var cacheService =  context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            // generate key for the request by using its parameters
            var key = generateKey(context.HttpContext.Request);

            // check if the response is cached or not by generated key
            string? response = await cacheService.GetCachedResponseAsync(key);
            if (!string.IsNullOrEmpty(response))
            {
                // return the cached response 
                var result = new ContentResult
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = result;
                return;
            }

            // if not cached 
            // invoke the endpoint then cache the result
            var executedContext = await next.Invoke();

            if (executedContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null )
            {
                // cache it 
                await cacheService.CacheResponseAsync(key, okObjectResult.Value , TimeSpan.FromSeconds(_timeToLive));
            }
            return;
        }

        // make FingerPrint Key 
        private string generateKey(HttpRequest request) // /api/Products?brandId=2&pageSize=2&pageIndex=2&sort=priceDesc
        {
            var key = new StringBuilder();

            // add path 
            key.Append(request.Path); // key: /api/Products

            foreach (var item in request.Query.OrderBy(q => q.Key)) 
            {
                key.Append($"|{item.Key}-{item.Value}");
            }

            return key.ToString();
        }
    }
}
