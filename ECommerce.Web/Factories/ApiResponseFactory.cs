using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var Errors = actionContext.ModelState
                        .Where(X => X.Value.Errors.Count > 0)
                        .ToDictionary(X => X.Key, X => X.Value.Errors.Select(X => X.ErrorMessage).ToArray());

            var Problem = new ProblemDetails()
            {
                Title = "Validation Error",
                Detail = "One Or More Validation Error Occurred",
                Status = StatusCodes.Status400BadRequest,
                Extensions =
                        {
                            { "Errors", Errors }
                        },
            };

            return new BadRequestObjectResult(Problem);
        }
    }
}
