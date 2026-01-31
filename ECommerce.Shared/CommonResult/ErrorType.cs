using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        Notfound = 2,
        Unauthorized = 3,
        Forbidden = 4,
        InvalidCredentials = 5
    }
}
