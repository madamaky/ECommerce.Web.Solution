using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        // Methods To Encapsulate 

        public static Error Failure(string Code = "General Failure", string Description = "General Failure Occurred")
            => new Error(Code, Description, ErrorType.Failure);

        public static Error Validation(string Code = "Validation Error", string Description = "One or More Validation Errors Occurred")
            => new Error(Code, Description, ErrorType.Validation);

        public static Error NotFound(string Code = "Not Found", string Description = "Error 404 Not Found")
            => new Error(Code, Description, ErrorType.Notfound);

        public static Error Unauthorized(string Code = "Unauthorized Error", string Description = "You Are Not Authorized To Access This Resource")
            => new Error(Code, Description, ErrorType.Unauthorized);

        public static Error Forbidden(string Code = "Forbidden Error", string Description = "Access To This Resource Is Forbidden")
            => new Error(Code, Description, ErrorType.Forbidden);

        public static Error InvalidCredentials(string Code = "Invalid Credentials", string Description = "The Provided Credentials Are Invalid")
            => new Error(Code, Description, ErrorType.InvalidCredentials);
    }
}
