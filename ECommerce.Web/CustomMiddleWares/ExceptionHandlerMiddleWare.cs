using ECommerce.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        // To Make ExceptionHandlerMiddleWare a MiddleWare
        // You Must Inject RequestDelegate As Next Middleware To Your CTOR
        // You Must InvokeAsync Method With HttpContext As Parameter
        
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate Next, ILogger<ExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                // 404 NotFound
                await HandleNotFoundEndPointAsync(context);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                _logger.LogError(ex, "Something went wrong");
                //await context.Response.WriteAsJsonAsync(new
                //{
                //    StatusCode = StatusCodes.Status500InternalServerError,
                //    Error = $"An unexpected error occurred: {ex.Message}"
                //});
                var Problem = new ProblemDetails()
                {
                    Title = "An Unexpected Error Occurred",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };
                context.Response.StatusCode = Problem.Status.Value;
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "Resources Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The Resources You Are Looking For Is Not Found",
                    Instance = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
