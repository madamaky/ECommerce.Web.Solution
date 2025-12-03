using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerce.Persentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        // Common Result
        // Handle Result

        // Handle Request Without Values:
        //     If Success => Return No Content (204)
        //     If Failure => Return Problem Details With Status Code, Error Details

        // Handle Request With Values:
        //     If Success => Return Ok (200) 
        //     If Failure => Return Problem Details With Status Code, Error Details

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent(); // 204 No Content
            else
                return HandleProblem(result.Errors);
        }

        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value); // 200 Ok
            else
                return HandleProblem(result.Errors);
        }

        protected string GetEmailFromToken() => User.FindFirstValue(ClaimTypes.Email)!;

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // If No Error => Return Default Error
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                               title: "An unexpected error occurred.",
                               detail: "No error details were provided.");

            // If More Than One Error => Handle As Validation 
            if (errors.All(E => E.Type == ErrorType.Validation))
                return HandleValidationProblem(errors);

            // If One Error => Handle It
            return HandleSingleErrorProblem(errors[0]);
        }

        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var ModelState = new ModelStateDictionary();

            foreach (var error in errors)
                ModelState.AddModelError(error.Code, error.Description);

            return ValidationProblem(ModelState);
        }

        private ActionResult HandleSingleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.Type));
        }

        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.Notfound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}
