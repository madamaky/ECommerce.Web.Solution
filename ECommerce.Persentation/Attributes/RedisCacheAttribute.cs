using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Persentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInMin;

        public RedisCacheAttribute(int DurationInMin = 5)
        {
            _durationInMin = DurationInMin;
        }

        //// Executed After EndPoint
        //public override void OnActionExecuted(ActionExecutedContext context)

        //// Executed Before EndPoint
        //public override void OnActionExecuting(ActionExecutingContext context)


        // Executed Asynchronously Before and After EndPoint
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get Cache Service
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // Create CacheKey
            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            // Check If Cached Data Exists
            var cacheValue = await cacheService.GetAsync(cacheKey);
            if(cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult result)
                await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(_durationInMin));
        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(X => X.Key))
                Key.Append($"|{item.Key}-{item.Value}");
            return Key.ToString();
        }
    }
}
