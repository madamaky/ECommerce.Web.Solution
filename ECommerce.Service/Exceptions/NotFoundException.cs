using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Exceptions
{
    // Using Primary CTOR
    public abstract class NotFoundException(string message) : Exception(message)
    {
        //protected NotFoundException(string message) : base(message) { }
    }

    public class ProductNotFoundException(int id) : NotFoundException($"Product With Id {id} Not Found")
    {
        //public ProductNotFoundException(int id) : base($"Product With Id {id} Not Found") { }
    }

    public class BasketNotFoundException(string id) : NotFoundException($"Basket With Id {id} Not Found")
    {
    }
}
