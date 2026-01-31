using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Service.Specification
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentSpecification(string intentId) : base(O => O.PaymentIntentId == intentId)
        {

        }
    }
}
