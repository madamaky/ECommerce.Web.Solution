using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Service.Specification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string email) : base(O => O.UserEmail == email)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }

        public OrderSpecification(Guid id, string email) : base(O => O.Id == id && (string.IsNullOrEmpty(email) || O.UserEmail.ToLower() == email.ToLower()))
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
        }
    }
}
